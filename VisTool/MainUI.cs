using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Threading;
using System.Collections.Specialized;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using Microsoft.Win32.TaskScheduler;
using Microsoft.Win32;

namespace VisTool
{
    public partial class MainUI : Form, ILog, IProgress 
    {
        public string m_url;

        bool m_bWaitModelTrainning = false;

        string m_connConfig;

        FileInfo m_dateFile;//保存标注数据的源文件路径

        CookieContainer m_cookies;//用于保存session

        serverConnector m_serverConnector;

        public string strIP, strPort, strUser, strPwd, strHDFSIP, strHDFSPort;
        
        public MainUI()
        {
            InitializeComponent();
            webBrowser1.Navigate(Application.StartupPath + "\\echart.html");
            
        }

        public void updateProgress(int val)
        {

        }

        public void printLog(string strLog)
        {
            MessageBox.Show(strLog);
        }
        
        private void 数据标注ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "csv文件|*.csv|All Files|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.FileName = "";
            string strFile = "";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                Thread threadDateUpload = new Thread(new ParameterizedThreadStart(threadDataUploadFun));
                threadDateUpload.Start(openFileDialog1.FileName); 
            }          
        }

        void threadDataUploadFun(object filePath)
        {
            if (m_serverConnector==null)
            {
                return;                
            }            
            //获取服务器端的响应
            m_serverConnector.saveSession();
            WebResponse wr = m_serverConnector.uploadCorpus2CalculateNode((string)filePath);
            if (wr==null)
            {
                MessageBox.Show("服务器没有响应");
                return;
            }
            //获取登录成功后的cookie
            Stream s = wr.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            //类型列表
            string strJson = sr.ReadLine();
            this.Invoke(new EventHandler(delegate {
                webBrowser1.Document.InvokeScript("DrawChart", new object[] { strJson });
            }));
                                
            while ((strJson = sr.ReadLine()) != null)
            {
                JArray ja = JArray.Parse(strJson);
                for (int i = 0; i < ja.Count; i++)
                {
                    ja[i]["value"] = Math.Log(double.Parse(ja[i]["value"].ToString()) * 2000 + 1);
                }
                this.Invoke(new EventHandler(delegate
                {
                    webBrowser1.Document.InvokeScript("SaveWordCloud", new object[] { ja.ToString() });
                }));                    
            }
            sr.Close();
            s.Close();
            wr.Close();               

            //XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
        }
        
        private void 存储ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFrm frm = new SaveFrm("模型名称");
            string strSaveURL = m_url + "/modelsave";
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                if (m_serverConnector == null)
                {
                    MessageBox.Show("未连接到服务器!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                HttpWebResponse response = (HttpWebResponse)m_serverConnector.httpGet2CalculateNode("/modelsave?name=" + frm.text);
                 if (response!=null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream ss = response.GetResponseStream();
                        StreamReader sr = new StreamReader(ss);
                        JObject json = JObject.Parse(sr.ReadToEnd());
                        sr.Close();
                        ss.Close();
                        sqlBuilder sb = new sqlBuilder();
                        sb.SetTable("models");
                        foreach (var col in json)
                        {
                            sb.AddColumn(col.Key.ToString(), col.Value.ToString());
                        }
                        string sql = sb.GetInsertSQL();
                        m_serverConnector.mysqlNonQuery(sql);

                        MessageBox.Show("模型 " + frm.text + " 保存成功！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {                        
                        MessageBox.Show("模型 " + frm.text + " 保存失败！", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    response.Close();  
                }
  
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void 预测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataUploadFrm frm = new DataUploadFrm(this);
            frm.ShowDialog();
        }

        void threadTrainFun(object trainningMethod)
        {
            if (m_serverConnector != null)
            {
                WebResponse response = null;
                Stream receiveStream = null;
                StreamReader readStream = null;
                try
                {
                    response = m_serverConnector.httpGet2CalculateNode("/train?method=" + trainningMethod.ToString());
                    receiveStream = response.GetResponseStream();
                    readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    string strTag = readStream.ReadLine();

                    string strAxis = readStream.ReadLine();
                    string strTrain = readStream.ReadLine();
                    string strTest = readStream.ReadLine();
                    //JArray ja = JArray.Parse(str);
                    this.Invoke(new EventHandler(delegate
                    {
                        webBrowser1.Document.InvokeScript("DrawTrainResult", new object[] { strTag,strAxis, strTrain, strTest });
                    }));
                                        
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if(readStream != null) readStream.Close();
                    if(receiveStream != null) receiveStream.Close();
                    if(response != null) response.Close();
                }

            }           
        }

        delegate void progressbarCallBack(int val);

        void updateProgressbar(int val)
        {
            if (this.progressBar.ProgressBar.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.progressBar.ProgressBar.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.progressBar.ProgressBar.Disposing || this.progressBar.ProgressBar.IsDisposed)
                        return;
                }
                progressbarCallBack d = new progressbarCallBack(updateProgressbar);
                this.progressBar.ProgressBar.Invoke(d, new object[] { val });
            }
            else
            {
                try
                {
                    this.progressBar.Value = val;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        
        private void cNNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread workThread = new Thread(new ParameterizedThreadStart(threadTrainFun));
            workThread.Start("CNN");

            statusInfo.Text = "服务器训练模型中";
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;
            progressBar.Step = 1;

            Application.DoEvents();

            while (m_bWaitModelTrainning)
            {
                this.progressBar.PerformStep();
                System.Threading.Thread.Sleep(500);
            }
            progressBar.Value = 100;
            statusInfo.Text = "模型训练完成";   
        }
        
        private void sVNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread workThread = new Thread(new ParameterizedThreadStart(threadTrainFun));
            workThread.Start("SVM");

            statusInfo.Text = "服务器训练模型中";
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;
            progressBar.Step = 1;

            Application.DoEvents();

            while (m_bWaitModelTrainning)
            {
                this.progressBar.PerformStep();
                System.Threading.Thread.Sleep(100);
            }
            progressBar.Value = 100;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.ScrollBarsEnabled = false;
            m_connConfig = Directory.GetCurrentDirectory() + "\\ServerConn.ini";

            m_serverConnector = new serverConnector(m_connConfig, this, this);

        }

        private void MainUI_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void 连接设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnFrm frm = new ConnFrm(m_connConfig);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                m_serverConnector = new serverConnector(m_connConfig, this, this);
            }
        }

    }
}

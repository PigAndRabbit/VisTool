using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;

namespace VisTool
{
    public partial class ConnFrm : Form
    {
        public string strIP, strPort, strUser, strPwd, strHDFSIP, strHDFSPort;

        public ConnFrm(string serverConnConfig)
        {
            InitializeComponent();

            StreamReader sr = new StreamReader(serverConnConfig);
            if (sr != null)
            {
                JObject ob = JObject.Parse(sr.ReadToEnd());

                mIPTB.Text = ob["mysql_host"].ToString();

                mPortTB.Text = ob["mysql_port"].ToString();

                tbDatabase.Text = ob["mysql_database"].ToString();

                mUserTB.Text = ob["mysql_user"].ToString();

                mPwdTB.Text = ob["mysql_pwd"].ToString();

                tbCode.Text = ob["mysql_charset"].ToString();

                tbHDFSAdd.Text = ob["hdfs_server"].ToString();

                JArray nodes = (JArray)ob["hdfs_nodes"];

                string[] arr = new string[nodes.Count];
                for (int i = 0; i < nodes.Count; i++)
                {
                    arr[i] = nodes[i]["name"].ToString() + ":" + nodes[i]["ip"];
                }

                tbHDFSNodes.Lines = arr;

                tbCalAdd.Text = ob["calculate_server"].ToString();

                tbGraph.Text = ob["graph_server"].ToString();

                sr.Close();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void butOK_Click(object sender, EventArgs e)
        {
            JObject ob = new JObject();
            ob.Add(new JProperty("mysql_host", mIPTB.Text));
            ob.Add(new JProperty("mysql_port", mPortTB.Text));
            ob.Add(new JProperty("mysql_user", mUserTB.Text));
            ob.Add(new JProperty("mysql_pwd", mPwdTB.Text));
            ob.Add(new JProperty("mysql_database", tbDatabase.Text));
            ob.Add(new JProperty("mysql_charset", tbCode.Text));
            ob.Add(new JProperty("calculate_server", tbCalAdd.Text));
            ob.Add(new JProperty("graph_server", tbGraph.Text));
            ob.Add(new JProperty("hdfs_server", tbHDFSAdd.Text));            

            JArray ja = new JArray();
            string[] strNodes = tbHDFSNodes.Lines;
            for(int i=0;i<strNodes.Length;i++)
            {
                string[] param = strNodes[i].Split(':');
                ja.Add(new JObject(new JProperty("name", param[0]), new JProperty("ip", param[1])));
            }
            ob.Add(new JProperty("hdfs_nodes", ja));


            StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\ServerConn.ini");
            sw.Write(ob.ToString());
            sw.Close();

            this.DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void MySQlConn_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //create mysql connection
            string strTemplate = "Database='{0}';Data Source={1};Port={2};User ID={3};Password={4};CharSet={5};";

            string strMysqlConn = string.Format(strTemplate, tbDatabase.Text, mIPTB.Text,
                mPortTB.Text, mUserTB.Text, mPwdTB.Text, tbCode.Text);

            serverConnector scTest = new serverConnector();
            string msg;
            if (!scTest.mysqlConnTest(strMysqlConn, out msg))
            {
                MessageBox.Show("MYSQL数据库连接失败："+msg, "测试连接", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (!scTest.hdfsConnTest(tbHDFSAdd.Text, out msg))
            {
                MessageBox.Show("HDFS连接失败：" + msg, "测试连接", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (!scTest.calNodeTest(tbCalAdd.Text, out msg))
            {
                MessageBox.Show("计算节点连接失败：" + msg, "测试连接", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (!scTest.graphNodeTest(tbGraph.Text, out msg))
            {
                MessageBox.Show("图计算节点连接失败：" + msg, "测试连接", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("连接成功！", "测试连接", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
         }
    }
}

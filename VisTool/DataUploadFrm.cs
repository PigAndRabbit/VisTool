using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Microsoft.Win32.TaskScheduler;

namespace VisTool
{
    public partial class DataUploadFrm : Form, ILog, IProgress 
    {
        public bool m_uploadData = false;

        public StreamWriter m_sw;

        public MainUI m_parent;

        public string m_selectedPath;//文件树中选中的文件/文件夹路径

        public string m_selectedModel;

        const string m_taskName = "VisToolUploadVideo";//定时任务名称    

        public serverConnector m_serverConnector;
        
        public static class IconIndexes
        {
            public const int MyComputer = 0;      //我的电脑  
            public const int ClosedFolder = 1;    //文件夹关闭  
            public const int OpenFolder = 2;      //文件夹打开  
            public const int FixedDrive = 3;      //磁盘盘符  
            public const int MyDocuments = 4;     //我的文档 
            public const int MyFile = 5;          //文件
        }
        
        /// <summary>  
        /// 自定义类TreeViewItems 调用其Add(TreeNode e)方法加载子目录  
        /// </summary>  
        public static class TreeViewItems
        {
            public static void Add(TreeNode e)
            {
                //try..catch异常处理  
                try
                {                    
                    //判断"我的电脑"Tag 上面加载的该结点没指定其路径  
                    if (e.Tag.ToString() != "我的电脑")
                    {
                        e.Nodes.Clear();                               //清除空节点再加载子节点  
                        TreeNode tNode = e;                            //获取选中\展开\折叠结点  
                        string path = tNode.Name;                      //路径    

                        //获取"我的文档"路径  
                        if (e.Tag.ToString() == "我的文档")
                        {
                            path = Environment.GetFolderPath           //获取计算机我的文档文件夹  
                                (Environment.SpecialFolder.MyDocuments);
                        }

                        //获取指定目录中的子目录名称并加载结点
                        string[] dics = Directory.GetDirectories(path);
                        foreach (string dic in dics)
                        {
                            TreeNode subNode = new TreeNode(new DirectoryInfo(dic).Name); //实例化  
                            subNode.Name = new DirectoryInfo(dic).FullName;               //完整目录  
                            subNode.Tag = subNode.Name;
                            subNode.ImageIndex = IconIndexes.ClosedFolder;       //获取节点显示图片  
                            subNode.SelectedImageIndex = IconIndexes.OpenFolder; //选择节点显示图片  
                            tNode.Nodes.Add(subNode);
                            subNode.Nodes.Add("");                               //加载空节点 实现+号  
                        }

                        //                         //获取指定目录下的文件
                        //                         string[] files = Directory.GetFiles(path,"*.*");
                        //                         foreach (string file in files)
                        //                         {
                        //                             TreeNode subNode = new TreeNode(new DirectoryInfo(file).Name); //实例化  
                        //                             subNode.Name = new DirectoryInfo(file).FullName;             //完整目录  
                        //                             subNode.Tag = subNode.Name;
                        //                             subNode.ImageIndex = IconIndexes.MyFile;       //获取节点显示图片  
                        //                             subNode.SelectedImageIndex = IconIndexes.MyFile; //选择节点显示图片  
                        //                             tNode.Nodes.Add(subNode);
                        //                             //subNode.Nodes.Add("");                             //加载空节点 实现+号  
                        //                         }
                    }
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.Message);                   //异常处理  
                }
            }
        }

        public DataUploadFrm(Form parent)
        {            
            m_parent = (MainUI)parent;
            InitializeComponent();
            
            //实例化TreeNode类 TreeNode(string text,int imageIndex,int selectImageIndex)              
            TreeNode rootNode = new TreeNode("我的电脑",
                IconIndexes.MyComputer, IconIndexes.MyComputer);  //载入显示 选择显示  
            rootNode.Tag = "我的电脑";                            //树节点数据  
            rootNode.Text = "我的电脑";                           //树节点标签内容  
            this.directoryTree.Nodes.Add(rootNode);               //树中添加根目录  

            //显示MyDocuments(我的文档)结点  
            var myDocuments = Environment.GetFolderPath           //获取计算机我的文档文件夹  
                (Environment.SpecialFolder.MyDocuments);
            TreeNode DocNode = new TreeNode(myDocuments);
            DocNode.Tag = "我的文档";                              //设置结点名称  
            DocNode.Text = "我的文档";
            DocNode.ImageIndex = IconIndexes.MyDocuments;         //设置获取结点显示图片  
            DocNode.SelectedImageIndex = IconIndexes.MyDocuments; //设置选择显示图片  
            rootNode.Nodes.Add(DocNode);                          //rootNode目录下加载节点
            DocNode.Nodes.Add("");

            //循环遍历计算机所有逻辑驱动器名称(盘符)  
            foreach (string drive in Environment.GetLogicalDrives())
            {
                //实例化DriveInfo对象 命名空间System.IO  
                var dir = new DriveInfo(drive);
                switch (dir.DriveType)           //判断驱动器类型  
                {
                    case DriveType.Fixed:        //仅取固定磁盘盘符 Removable-U盘   
                        {
                            //Split仅获取盘符字母  
                            TreeNode tNode = new TreeNode(dir.Name.Split(':')[0]);
                            tNode.Name = dir.Name;
                            tNode.Tag = tNode.Name;
                            tNode.ImageIndex = IconIndexes.FixedDrive;         //获取结点显示图片  
                            tNode.SelectedImageIndex = IconIndexes.FixedDrive; //选择显示图片  
                            directoryTree.Nodes.Add(tNode);                    //加载驱动节点  
                            tNode.Nodes.Add("");
                        }
                        break;
                }
            }
            rootNode.Expand();                  //展开树状视图  
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.Expand();
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeViewItems.Add(e.Node);
        }

        private void 上传ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logInfo.Clear();

            Thread threadUploadVideo = new Thread(new ThreadStart(threadUploadVideoFun));

            threadUploadVideo.Start();
        }

        void threadUploadVideoFun()
        {
            if (m_selectedModel == "")
            {
                MessageBox.Show("请先选择模型！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (m_selectedPath == "")
            {
                MessageBox.Show("请选择要上传的文件夹！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            m_serverConnector.updateVideoDatabase(m_selectedPath, m_selectedModel);
            return;

            HDFSDirFrm frm = new HDFSDirFrm();
            string hdfsPath;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                hdfsPath = frm.HDFSPath;
                m_serverConnector.uploadFolder2Server(m_selectedPath, hdfsPath, m_selectedModel);
            }          

        }

        void threadDailyUploadFun(object timeForm)
        {
            DateSelectFrm frm = (DateSelectFrm)timeForm;

            DateTime dtStart = frm.m_dtStart;
            DateTime dtEnd = frm.m_dtEnd;
            
            printLog("#" + DateTime.Now.ToString() + " 开始定时任务\n");
            string strlog;
            switch (frm.m_eType)
            {
                case MissionType.PerDay:
                    strlog = "# 任务周期：每日\n";
                    break;
                case MissionType.PerWeek:
                    strlog = "# 任务周期：每周\n";
                    break;
                case MissionType.PerMonth:
                    strlog = "# 任务周期：每月\n";
                    break;
                default:
                    strlog = "";
                    break;
            }
            printLog(strlog);

            printLog("# 任务起止日期：" + dtStart.ToLongDateString() + "-" + dtEnd.ToLongDateString() + "\n");

            printLog("# 定时上传时间：" + dtStart.ToLongTimeString() + "\n");

            printLog("\n");

            if (!frm.m_bStartNow)
            {
                while (DateTime.Now.CompareTo(frm.m_dtStart) < 0)
                {
                    Thread.Sleep(DateTime.Now - frm.m_dtStart);
                }
            }
            else
            {
                printLog(DateTime.Now.ToString() + " 开始本日上传任务...\n");
                //do upload
                threadUploadVideoFun();
                printLog(DateTime.Now.ToString() + " 本日上传任务完成\n");
                printLog("\n");
            }

            //loop daily task
            while (DateTime.Now.CompareTo(frm.m_dtEnd) <= 0)
            {
                switch (frm.m_eType)
                {
                    case MissionType.PerDay:
                        Thread.Sleep(TimeSpan.FromDays(1));
                        break;
                    case MissionType.PerWeek:
                        Thread.Sleep(TimeSpan.FromDays(7));
                        break;
                    case MissionType.PerMonth:
                        Thread.Sleep(dtStart.AddMonths(1) - dtStart);
                        break;
                    default:
                        break;
                }
                printLog(DateTime.Now.ToString() + " 开始本日上传任务...\n");
                //do upload
                printLog(DateTime.Now.ToString() + " 本日上传任务完成\n");
                printLog("\n");
            }
            printLog(DateTime.Now.ToString() + " 全部定时上传任务完成！\n");
            printLog("\n");
        }

        delegate void SetTextCallback(string text);

        public void printLog(string strLog)
        {
            strLog += "\n";
            if (this.logInfo.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.logInfo.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.logInfo.Disposing || this.logInfo.IsDisposed)
                        return;
                }
                SetTextCallback d = new SetTextCallback(printLog);
                this.logInfo.Invoke(d, new object[] { strLog });
            }
            else
            {
                try
                {
                    this.logInfo.AppendText(strLog);
                    this.m_sw.WriteLine(strLog);
                }
                catch
                {

                }
            }
        }

        delegate void progressbarCallBack(int val);

        public void updateProgress(int val)
        {
            if (this.progressBar.ProgressBar.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.progressBar.ProgressBar.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.progressBar.ProgressBar.Disposing || this.progressBar.ProgressBar.IsDisposed)
                        return;
                }
                progressbarCallBack d = new progressbarCallBack(updateProgress);
                this.progressBar.ProgressBar.Invoke(d, new object[] { val });
            }
            else
            {
                try
                {
                    this.progressBar.Value = val;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void DataUploadFrm_Load(object sender, EventArgs e)
        {
            //init variables
            m_selectedModel = "";

            string connConfig = Directory.GetCurrentDirectory() + "\\ServerConn.ini";

            m_serverConnector = new serverConnector(connConfig, this, this);
            
            //log
            string strLogPath = Directory.GetCurrentDirectory() + "\\log";
            if (!Directory.Exists(strLogPath))
            {
                Directory.CreateDirectory(strLogPath);
            }

            string strLogName = DateTime.Now.ToLongDateString() + ".txt";

            string strFullName = strLogPath + "\\" + strLogName;

            if (!File.Exists(strFullName))
            {
                File.Create(strFullName).Close();
            }

            m_sw = new StreamWriter(strFullName, true);                          

        }

        private void DataUploadFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_sw != null)
            {
                m_sw.Close();
            }
            m_serverConnector.mysqlClose();
        }
        
        private void 删除HDFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFrm frm = new SaveFrm("HDFS文件夹路径");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                string strURL;
                //删除已有文件
                strURL = "http://10.168.103.106:50070/webhdfs/v1/";
                strURL += frm.text;
                strURL += "?user.name=hadoop&op=DELETE&recursive=true";

                HttpWebRequest reqDel = (HttpWebRequest)WebRequest.Create(strURL);
                reqDel.Method = "DELETE";
                reqDel.Timeout = 300000;
                HttpWebResponse resDel = (HttpWebResponse)reqDel.GetResponse();
                MessageBox.Show(resDel.StatusCode.ToString());
            }
        }
        
        private void 选择模型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelList frm = new ModelList(this);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.m_selectedModelName!="")
                {
                    m_selectedModel = frm.m_selectedModelName;
                }                
            }
        }

        private void directoryTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            m_selectedPath = directoryTree.SelectedNode.Name;
        }

        private void 停止ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskService ts = new TaskService();
            Microsoft.Win32.TaskScheduler.Task task = ts.FindTask(m_taskName);
            if (task != null)
            {
                ts.RootFolder.DeleteTask(m_taskName);
            }
            printLog("#定时任务已终止！");
        }

        private void 创建定时上传ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_selectedModel == "")
            {
                MessageBox.Show("请先选择模型！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (m_selectedPath == "")
            {
                MessageBox.Show("请选择要上传的文件所在的文件夹！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TaskService ts = new TaskService();
            Microsoft.Win32.TaskScheduler.Task task = ts.FindTask(m_taskName);
            if (task != null)
            {
                ts.RootFolder.DeleteTask(m_taskName);
            }
            // Create a new task
            TimeTrigger trigger = new TimeTrigger();
            trigger.StartBoundary = DateTime.Now.AddSeconds(10);
            trigger.EndBoundary = DateTime.Now.AddMonths(1);
            trigger.Repetition.Interval = TimeSpan.FromMinutes(1);
            trigger.Repetition.Duration = TimeSpan.FromMinutes(3);

            string curDir = System.Environment.CurrentDirectory;

            string argument;

            argument = Directory.GetCurrentDirectory() + "\\ServerConn.ini " + m_selectedPath + " " + m_selectedModel;

            ExecAction action = new ExecAction("SchedulerUpload.exe",argument, curDir);
            
            task = ts.AddTask(m_taskName, trigger, action);

            // Edit task and re-register if user clicks Ok
            TaskEditDialog editorForm = new TaskEditDialog();
            editorForm.Editable = true;
            editorForm.RegisterTaskOnAccept = true;
            editorForm.Initialize(task);
            editorForm.AvailableTabs = AvailableTaskTabs.Triggers | AvailableTaskTabs.RunTimes | AvailableTaskTabs.Settings | AvailableTaskTabs.History | AvailableTaskTabs.Conditions;
            editorForm.Title = "创建定时任务";
            // ** The four lines above can be replaced by using the full constructor
            // TaskEditDialog editorForm = new TaskEditDialog(t, true, true);
            editorForm.ShowDialog();

            printLog("#开始定时上传任务");
            //AvailableTaskTabs aa;

            // Remove the task we just created




            //             DateSelectFrm frm = new DateSelectFrm();
            // 
            //             if (frm.ShowDialog(this) == DialogResult.OK)
            //             {
            //                 Thread threadDailyUpload = new Thread(new ParameterizedThreadStart(threadDailyUploadFun));
            //                 threadDailyUpload.Start(frm);
            //             }
            // 
            //             TaskDefinition td = TaskService.Instance.NewTask();
            //             td.RegistrationInfo.Description = "schedularUpload";
            //             
            //             WeeklyTrigger wt = new WeeklyTrigger();
            //             wt.StartBoundary = DateTime.Now.AddSeconds(5);
            //             //wt.DaysOfWeek = DaysOfTheWeek.Monday | DaysOfTheWeek.Saturday;
            //             // wt.WeeksInterval = 2;
            //             wt.Repetition.Duration = TimeSpan.FromHours(11);
            //             wt.Repetition.Interval = TimeSpan.FromMinutes(10);
            //             td.Triggers.Add(wt);
            // 
            //             // Create an action that will launch Notepad whenever the trigger fires
            //             td.Actions.Add("D:\\Works_ISA\\VisTool\\bin\\Debug\\SchedulerUpload.exe", "D:\\什么");
            // 
            //             // Register the task in the root folder of the local machine
            //             TaskService.Instance.RootFolder.RegisterTaskDefinition("Test1", td);
        }

        private void directoryTree_Click(object sender, EventArgs e)
        {
            m_selectedPath = directoryTree.SelectedNode.Name;
        }

        private void directoryTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            m_selectedPath = directoryTree.SelectedNode.Name;
        }
        
    }
    
}

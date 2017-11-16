using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Microsoft.Win32.TaskScheduler;

namespace VisTool
{
    public interface ILog
    {
        void printLog(string strLog);
    }

    public interface IProgress
    {
        void updateProgress(int value);
    }

    public class serverConnector
    {
        MySqlConnection m_mysqlConn;

        Dictionary<string, string> m_hdfsNodeList;

        string m_hdfsServerUrl;

        string m_calServerUrl;

        string m_graphServerUrl;

        int m_taskPercentage;

        ILog m_log;//输出日志信息的接口 

        IProgress m_progress;//输出任务进度的接口

        CookieContainer m_cookies;//用于保存session

        bool m_bSaveSession;//是否保存session

        public serverConnector(string serverConnConfig, ILog log = null, IProgress progress =null)
        {
            //init variables
            m_log = log;
            m_progress = progress;

            SetAllowUnsafeHeaderParsing(true);

            //connect server
            StreamReader sr = new StreamReader(serverConnConfig);
            if(sr != null)
            {
                JObject ob = JObject.Parse(sr.ReadToEnd());

                //create mysql connection
                string strTemplate = "Database='{0}';Data Source={1};Port={2};User ID={3};Password={4};CharSet={5};";

                string strMysqlConn = string.Format(strTemplate, ob["mysql_database"], ob["mysql_host"],
                    ob["mysql_port"], ob["mysql_user"], ob["mysql_pwd"], ob["mysql_charset"]);

                m_mysqlConn = new MySqlConnection(strMysqlConn);

                try
                {
                    m_mysqlConn.Open();
                }
                catch (System.Exception ex)
                {
                    m_log.printLog(ex.Message);
                }
                m_mysqlConn.Close();

                //create hdfs connection
                m_hdfsServerUrl = ob["hdfs_server"].ToString();

                m_hdfsNodeList = new Dictionary<string, string>();
                JArray nodes = (JArray)ob["hdfs_nodes"];
                foreach(JObject node in nodes)
                {
                    m_hdfsNodeList.Add(node["name"].ToString(), node["ip"].ToString());
                }

                //create calculate server connection
                m_calServerUrl = ob["calculate_server"].ToString();

                m_graphServerUrl = ob["graph_server"].ToString();
                
                sr.Close();
            }            
        }

        public serverConnector()
        {
            SetAllowUnsafeHeaderParsing(true);
        }

        public bool mysqlConnTest(string mysqlConn,out string message)
        {
            MySqlConnection conn = new MySqlConnection(mysqlConn);
            try
            {
                conn.Open();
            }
            catch (System.Exception ex)
            {
                message = ex.Message;
                return false;
            }
            conn.Close();
            message = "ok";
            return true;
        }

        public bool hdfsConnTest(string hdfsConn, out string message)
        {
            string msg="";
            HttpWebResponse res = (HttpWebResponse)httpGET(hdfsConn,out msg);
            if (res != null && res.StatusCode == HttpStatusCode.OK)
            {
                message = "ok";
                return true;
            }
            else
            {
                message = msg;
                return false;
            }
        }

        public bool calNodeTest(string calNodeConn, out string message)
        {
            string msg;
            HttpWebResponse res = (HttpWebResponse)httpGET(calNodeConn, out msg);
            if (res != null && res.StatusCode == HttpStatusCode.OK)
            {
                message = "ok";
                return true;
            }
            else
            {
                message = msg;
                return false;
            }
        }

        public bool graphNodeTest(string graphNodeConn, out string message)
        {
            string msg;
            HttpWebResponse res = (HttpWebResponse)httpGET(graphNodeConn, out msg);
            if (res != null && res.StatusCode == HttpStatusCode.OK)
            {
                message = "ok";
                return true;
            }
            else
            {
                message = msg;
                return false;
            }
        }

        public WebResponse httpUploadFile(string url, string file, string paramName, string contentType)
        {
            string strRespons = "";
            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            wr.AllowWriteStreamBuffering = false;

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

            byte[] trailer = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            wr.ContentLength = fileStream.Length+headerbytes.Length+trailer.Length+boundarybytes.Length;
            if (m_cookies!=null)
            {                
                wr.CookieContainer = m_cookies;
            }
            

            Stream rs = wr.GetRequestStream();
            rs.Write(boundarybytes, 0, boundarybytes.Length);


            rs.Write(headerbytes, 0, headerbytes.Length);

            
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();


            rs.Write(trailer, 0, trailer.Length);
            rs.Close();
            WebResponse res = null;
            try
            {
                res= wr.GetResponse();
                if (m_bSaveSession)
                {                    
                    //m_cookies = new CookieContainer();
//                     string cookieheader = wr.cookiecontainer.getcookieheader(new uri(url));
//                     m_cookies.setcookies(new uri(url), cookieheader);
                    m_cookies = wr.CookieContainer;
                    m_bSaveSession = false;
                }                
                //string str = reader2.ReadLine();
                //MessageBox.Show(string.Format("File uploaded, server response is: {0}", responseStreamReader.ReadToEnd()));
            }
            catch (Exception ex)
            {
                if(m_log!=null)
                m_log.printLog(ex.Message);
            }
//             if (wr != null)
//             {
//                 wr.Abort();
//             }
            return res;
        }

        public bool hdfsMakeDirectory(string strDirectory)
        {
            //创建HDFS目录
            string strURL = m_hdfsServerUrl + "/webhdfs/v1/";
            strURL += strDirectory;
            strURL += "?user.name=hadoop&op=MKDIRS&permission=755";
   
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "PUT";
            request.Timeout = 5000;
            request.KeepAlive = false;
            HttpWebResponse response=null;
            bool bSuccess = false;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    bSuccess = true;
                }
            }
            catch (Exception ex)
            {
                if (m_log != null)
                {
                    m_log.printLog(ex.Message);
                }
            }
            if (response != null)
            {
                response.Close();
            }
            if (request != null)
            {
                request.Abort();
            }
            return bSuccess;
        }


        //向服务器上传指定的库文件夹
        //folder：库文件夹路径
        //modelName：使用的视频分析模型名称
        public void updateVideoDatabase(string folder, string modelName)
        {
            bool bStatus = true;//任务状态，出现任何错误，则返回false
            string errMsg = "";

            DirectoryInfo dbDir = new DirectoryInfo(folder);
            string dbPath = dbDir.Name;
            if (!hdfsMakeDirectory(dbPath))
            {
                if (m_log != null) m_log.printLog("# 服务器创建文件夹" + dbPath + "失败！");
                return;
            }
            //遍历本地任务文件夹并创建服务器上的任务文件夹 
            foreach (DirectoryInfo taskDir in dbDir.GetDirectories("*"))
            {
                string taskPath = dbPath + "/" + taskDir.Name;
                if (!hdfsMakeDirectory(taskPath))
                {
                    if (m_log != null) m_log.printLog("# 服务器创建文件夹" + taskPath + "失败！");
                    return;
                }                
            }

            if (m_log != null)
            {
                m_log.printLog("# " + DateTime.Now.ToString() + " 开始上传文件夹到服务器");
                m_log.printLog("");
            }
            string strDateFolder = DateTime.Now.ToString("yyyy-MM-dd");
            foreach (DirectoryInfo taskDir in dbDir.GetDirectories("*"))
            {
                string taskPath = dbPath + "/" + taskDir.Name;
                DirectoryInfo[] dateDir = taskDir.GetDirectories(strDateFolder);
                if (dateDir.Length == 1)
                {
                    string strDatePath = taskPath + "/" + dateDir[0].Name;
                    bStatus = uploadFolder2Server(dateDir[0].FullName, strDatePath, modelName);
                }
            }

            //向计算节点发送更新视频信息请求
            string strParam = "/undergo?name=" + modelName;
            HttpWebResponse res = (HttpWebResponse)httpGet2CalculateNode(strParam);

            if (res != null && res.StatusCode == HttpStatusCode.OK)
            {
                res.Close();
                HttpWebResponse res2 = (HttpWebResponse)httpGET(m_graphServerUrl+"/VideoRel/update");
                if (res2 != null && res2.StatusCode != HttpStatusCode.OK)
                {
                    if (m_log != null)
                    {
                        m_log.printLog("向图数据库发送消息失败！");
                    }
                }
            }
            else
            {
                if (m_log != null)
                {
                    m_log.printLog("向计算节点发送消息失败！");
                }
            }

            if (m_log != null && bStatus)
            {
                m_log.printLog("# 上传视频库文件夹到服务器完成");
            }
        }

        //上传指定的任务文件夹到服务器
        public bool uploadFolder2Server(string strFromPath, string strToPath, string modelName)
        {
            try
            {
                m_mysqlConn.Open();
            }
            catch (System.Exception ex)
            {
                if(m_log !=null) m_log.printLog(ex.Message);
                return false;
            }

            if (!hdfsMakeDirectory(strToPath))
            {
                m_log.printLog("# 服务器创建文件夹" + strToPath + "失败！");
                return false;
            }
            else
            {
                if (m_log != null)
                {
                    m_log.printLog(DateTime.Now.ToString() + " 创建目录" + strToPath + "成功");
                    m_log.printLog("");
                }
            }

            //创建任务记录
            string[] strTaskInfo = Directory.GetFiles(strFromPath, "task_info.json");
            int code;
            string errMsg = "";

            if (strTaskInfo.Length == 1)
            {
                StreamReader sr = new StreamReader(strTaskInfo[0]);
                string strBuffer = sr.ReadToEnd();
                JObject obj = JObject.Parse(strBuffer);
                sqlBuilder sb = new sqlBuilder();
                sb.SetTable("tb_task");
                sb.AddColumn("task_state", "未导入");
                sb.AddColumn("task_begin", DateTime.Now.ToString());
                if (obj["title"] != null)
                {
                    sb.AddColumn("task_title", obj["title"].ToString());
                }
                if (obj["time"] != null)
                {
                    DateTime dt;
                    if (DateTime.TryParse(obj["time"].ToString(), out dt))
                    {
                        sb.AddColumn("task_time", dt.ToString("yyyy-MM-dd"));
                    }
                }
                if (obj["keywords"] != null)
                {
                    JArray ja = (JArray)obj["keywords"];
                    string temp = ja[0].ToString();
                    for (int i = 1; i < ja.Count; i++)
                    {
                        temp += "," + ja[i];
                    }
                    sb.AddColumn("task_key", escape(temp));
                }
                if (obj["file_size"] != null)
                {
                    sb.AddColumn("task_size", obj["file_size"].ToString());
                }
                if (obj["file_number"] != null)
                {
                    sb.AddColumn("task_number", obj["file_number"].ToString());
                }

                sb.AddColumn("error_message", errMsg);
                string sql = sb.GetInsertSQL();
                MySqlCommand cmd = new MySqlCommand(sql, m_mysqlConn);
                code = cmd.ExecuteNonQuery();
                long task_id = cmd.LastInsertedId;

                if (m_log != null && code == -1)
                {
                    m_log.printLog("Mysql更新任务表失败！");
                }
                else
                {
                    int total_cnt = 0, valid_cnt = 0;
                    bool bStatus = true;//任务状态，出现任何错误，则返回false                    
                    StreamWriter sw = null;
                    try
                    {
                        //过滤视频数据
                        string[] strFiles = Directory.GetFiles(strFromPath, "*.*", SearchOption.TopDirectoryOnly);
                        var files = strFiles.Where(s => s.EndsWith(".flv") || s.EndsWith(".mp4") || s.EndsWith(".wmv") || s.EndsWith(".mpg") || s.EndsWith(".avi"));
                        //var files = strFiles.Where(s => s.EndsWith(".json"));
                        total_cnt = strFiles.Length;
                        //创建上传失败文件夹
                        string failPath = strFromPath + "\\upload_failed";
                        Directory.CreateDirectory(failPath);
                        //创建上传失败日志
                        sw = new StreamWriter(failPath + "\\log.txt", false);

                        string hdfsMsg, mysqlMsg;
                        bool bhdfsSuccess, bmysqlSuccess;
                        foreach (string file in files)
                        {
                            //上传文件
                            hdfsMsg = "";
                            mysqlMsg = "";
                            bhdfsSuccess = uploadFile2HDFS(file, strToPath, ref hdfsMsg);
                            bhdfsSuccess = true;
                            //把视频元数据信息上传到mysql

                            bmysqlSuccess = uploadVideoMeta2MySQL(file, bhdfsSuccess, hdfsMsg, ref mysqlMsg, task_id);

                            if (bhdfsSuccess & bmysqlSuccess == false)
                            {
                                sw.WriteLine(Path.GetFileName(file));
                                sw.WriteLine(hdfsMsg);
                                sw.WriteLine("\n");
                                sw.WriteLine(mysqlMsg);
                                sw.WriteLine("\n");

                                //上传失败,移动视频到失败目录，保存日志
                                string fullFileToPath = failPath + "\\" + Path.GetFileName(file);
                                File.Move(file, fullFileToPath);
                                //移动视频元文件到失败目录
                                string metaName = Path.GetFileNameWithoutExtension(file) + ".json";
                                string oldMetaPath = Path.GetDirectoryName(file) + "\\" + metaName;
                                string fullMetaToPath = failPath + "\\" + metaName;
                                File.Move(oldMetaPath, fullMetaToPath);
                            }
                            else
                            {
                                valid_cnt++;
                            }
                        }
                        //update tb_task
                        sqlBuilder sqlUpdate = new sqlBuilder();
                        sqlUpdate.SetTable("tb_task");
                        sqlUpdate.AddColumn("task_number", valid_cnt.ToString());
                        sqlUpdate.AddColumn("task_substate", "0/" + valid_cnt.ToString());
                        sqlUpdate.AddColumn("task_end", DateTime.Now.ToString());
                        sqlUpdate.AddCondition("id", task_id.ToString());
                        sql = sqlUpdate.getUpdateSQL();
                        cmd = new MySqlCommand(sql, m_mysqlConn);
                        code = cmd.ExecuteNonQuery();

                        sw.Close();
                        if (m_mysqlConn != null)
                        {
                            m_mysqlConn.Close();
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        bStatus = false;
                        errMsg = ex.Message;

                        if (m_log != null)
                            m_log.printLog(ex.Message);

                        if (sw != null)
                        {
                            sw.Close();
                        }
                    }
                }                
            }
            else
            {
                m_log.printLog("找不到task_info.json文件！");
            }

            if (m_mysqlConn != null)
            {
                m_mysqlConn.Close();
            }
            return false;
        }

        //上传指定的视频文件到hdfs服务器
        //上传成功返回true，info中返回文件在hdfs服务器上的地址
        //上传失败返回false，info中返回错误信息
        public bool uploadFile2HDFS(string strFile, string strToPath,ref string info)
        {
            HttpWebRequest httpReq1=null, httpReq=null;
            HttpWebResponse res1 = null, res = null;
            int total=0;
            try
            {                
                FileInfo fi = new FileInfo(strFile);
                //strToPath = "data";
                //step1
                string uuid = System.Guid.NewGuid().ToString("N");
                string urlPath = strToPath + "/" + uuid;
                string strURL = m_hdfsServerUrl+"/webhdfs/v1/" + WebUtility.UrlEncode(urlPath) + "?user.name=hadoop&op=CREATE&overwrite=true";
                //string strURL = m_hdfsServerUrl + "/webhdfs/v1/" + strToPath + "/" + fi.Name + "?user.name=hadoop&op=CREATE&overwrite=true";
                //strURL+="?op=CREATE[&overwrite=<true|false>][&blocksize=<LONG>][&replication=<SHORT>][&permission=<OCTAL>][&buffersize=<INT>]";
//                 byte[] strTemp=Encoding.Default.GetBytes(strURL);
//                 string strUTF8URL = Encoding.UTF8.GetString(strTemp);
                httpReq1 = WebRequest.Create(strURL) as HttpWebRequest;
                httpReq1.Method = "PUT";
                httpReq1.AllowAutoRedirect = false;
                httpReq1.Timeout = 300000;
                httpReq1.KeepAlive = false;

                res1 = (HttpWebResponse)httpReq1.GetResponse();
                if (res1.StatusCode != HttpStatusCode.TemporaryRedirect)
                {
                    info = "HDFS服务器分配节点失败！";
                    if (res1 != null)
                    {
                        res1.Close();
                    }
                    if (httpReq1 != null)
                    {
                        httpReq1.Abort();
                    }
                    return false;
                }

                string _url = res1.Headers["Location"];

                string nodeName = _url.Substring(7, _url.IndexOf(':', 8) - 7);

                if (m_hdfsNodeList[nodeName] == null)
                {                    
                    info = "找不到HDFS节点" + nodeName;
                    if (m_log != null)
                    {
                        m_log.printLog(info);
                    }
                    if (res1 != null)
                    {
                        res1.Close();
                    }
                    if (httpReq1 != null)
                    {
                        httpReq1.Abort();
                    }
                    return false;
                }
                _url = _url.Replace(nodeName, m_hdfsNodeList[nodeName]);

                string url2 = _url.Insert(_url.IndexOf('?') + 1, "user.name=hadoop&");
                                
                if (m_log != null)
                m_log.printLog(DateTime.Now.ToString() + " HDFS服务器分配节点\n");

                if (res1 != null)
                {
                    res1.Close();
                }
                if (httpReq1 != null)
                {
                    httpReq1.Abort();
                }
                //==========================================================================
                //step2

                FileStream fs = new FileStream(strFile, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(fs);

                //时间戳
                string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");
                //byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");
                //请求头部信息
                StringBuilder sb = new StringBuilder();
                sb.Append("--");
                sb.Append(strBoundary);
                sb.Append("\r\n");
                sb.Append("Content-Disposition: form-data; name=\"");
                sb.Append("file");
                sb.Append("\"; filename=\"");
                sb.Append("csvdata.txt");
                sb.Append("\"");
                sb.Append("\r\n");
                sb.Append("Content-Type: ");
                sb.Append("application/octet-stream");
                sb.Append("\r\n");
                sb.Append("\r\n");
                string strPostHeader = sb.ToString();
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);

                byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "--\r\n");
                // 根据uri创建HttpWebRequest对象

                httpReq = (HttpWebRequest)WebRequest.Create(new Uri(url2));
                httpReq.Method = "PUT";
                //对发送的数据不使用缓存
                //httpReq.AllowWriteStreamBuffering = false;
                //设置获得响应的超时时间（300秒）
                httpReq.Timeout = System.Threading.Timeout.Infinite;
                httpReq.KeepAlive = false;
                httpReq.AllowWriteStreamBuffering = false;

                httpReq.ContentType = "multipart/form-data; boundary=" + strBoundary;
                httpReq.ContentLength = fs.Length;
                long length = fs.Length + postHeaderBytes.Length + trailer.Length;
                long fileLength = fs.Length;
                //httpReq.ContentLength = length;

                //每次上传4k
                int bufferLength = 4096;
                byte[] buffer = new byte[bufferLength];
                //已上传的字节数
                long offset = 0;
                //开始上传时间
                if (m_log != null)
                    m_log.printLog(DateTime.Now.ToString() + " 开始向节点" + nodeName + "上传数据 " + strFile + "\n");
                int size = 0;
                Stream postStream = httpReq.GetRequestStream();
                //发送请求头部消息
                //postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

                while ((size = r.Read(buffer, 0, buffer.Length)) != 0)
                {
                    postStream.Write(buffer, 0, size);
                    total += size;
                    //                     offset += size;
                    //                     if (m_progress != null)
                    //                     {
                    //                         m_progress.updateProgress((int)(offset * (100 / length)));
                    //                     }
                    //Application.DoEvents();
                }

                //添加尾部的时间戳
                
                //postStream.Write(trailer, 0, trailer.Length);
                postStream.Close();
                fs.Close();
                r.Close();


                //获取服务器端的响应
                res = (HttpWebResponse)httpReq.GetResponse();
                info = _url.Substring(0, _url.IndexOf('/', 7)+1) + "webhdfs/v1/" + strToPath+"/" + fi.Name;

                if (m_log != null)
                {
                    m_log.printLog(DateTime.Now.ToString() + " 数据上传完成，服务器路径" + info);
                    m_log.printLog("");
                }
                if (res != null)
                {
                    res.Close();
                }
                if (httpReq != null)
                {
                    httpReq.Abort();
                }
                return true;
                //Thread.Sleep(10000);
            }
            catch (Exception e)
            {
                if (m_log != null)
                {
                    m_log.printLog(DateTime.Now.ToString() + " 上传文件错误: " + e.Message);
                    m_log.printLog("");
                    info = e.Message;
                }
                return false;
            }
        }

        //title: 上传视频数据的元数据信息到mysql
        //videoPath:视频文件所在目录
        //bSuccess:上传到hdfs服务器是否成功
        //info:当bSuccess为true时，info为视频在hdfs服务器上的地址，当bSuccess为false时，info为错误信息
        //返回上传到mysql是否成功
        public bool uploadVideoMeta2MySQL(string videoPath,bool bSuccess,string info,ref string message,long task_id)
        {
            bool bMysqlSuccess=true;
            if (m_log != null)
            {
                m_log.printLog(DateTime.Now.ToString() + " 开始保存视频元数据信息到MYSQL数据库");

            }
            System.Diagnostics.Process p;
            JObject videoMeta, videoStreams, videoFormat, spiderMeta;
            JArray streams;
            StreamReader sr;//爬虫信息
            string sql,errMsg="";
            sqlBuilder sb = new sqlBuilder();

            sb.SetTable("tb_video_meta");
            
            //获取视频元数据信息
            try
            {
                p = new System.Diagnostics.Process();
                p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                string path = Directory.GetCurrentDirectory();
                string cmdline = "ffprobe -v quiet -print_format json -show_format -show_streams \"" + videoPath + "\"&exit";
                p.StandardInput.WriteLine(cmdline);
                p.StandardInput.AutoFlush = true;
                p.StandardOutput.ReadLine();
                p.StandardOutput.ReadLine();
                p.StandardOutput.ReadLine();
                p.StandardOutput.ReadLine();
                string buffer = p.StandardOutput.ReadToEnd();

                sb.AddColumn("state", "未导入");
                sb.AddColumn("task_id", task_id.ToString());

                videoMeta = JObject.Parse(buffer);
                if (videoMeta["streams"] != null)
                {
                    streams = (JArray)videoMeta["streams"];
                    videoStreams = (JObject)streams[0];
                    if (videoStreams["r_frame_rate"] != null)
                    {
                        sb.AddColumn("fps", videoStreams["r_frame_rate"].ToString());
                    }
                    if (videoStreams["codec_name"] != null)
                    {
                        sb.AddColumn("codec", escape(videoStreams["codec_name"].ToString()));
                    }
                    if (videoStreams["bit_rate"] != null)
                    {
                        sb.AddColumn("bit_rate", videoStreams["bit_rate"].ToString());
                    }
                    if (videoStreams["width"].ToString() != null)
                    {
                        sb.AddColumn("width", videoStreams["width"].ToString());
                    }
                    if (videoStreams["height"] != null)
                    {
                        sb.AddColumn("height", videoStreams["height"].ToString());
                    }                    
                    string strClarity;
                    if (int.Parse(videoStreams["width"].ToString()) < 720)
                    {
                        strClarity = "标清";
                    }
                    else if (int.Parse(videoStreams["width"].ToString()) < 1080)
                    {
                        strClarity = "高清";
                    }
                    else
                    {
                        strClarity = "超清";
                    }
                    sb.AddColumn("clarity", strClarity);

                }
                if (videoMeta["format"] != null)
                {
                    videoFormat = (JObject)videoMeta["format"];
                    sb.AddColumn("size", videoFormat["size"].ToString());
                    sb.AddColumn("duration", videoFormat["duration"].ToString());
                    sb.AddColumn("format", escape(videoFormat["format_name"].ToString()));
                }                
                
                //获取爬虫信息
                string strSpiderMeta = Path.GetDirectoryName(videoPath) + "\\" + Path.GetFileNameWithoutExtension(videoPath) + ".json";
                sr = new StreamReader(strSpiderMeta);
                string strBuffer = sr.ReadToEnd();
                
                if (strBuffer != "")
                {
                    spiderMeta = JObject.Parse(strBuffer);
                    if (spiderMeta["keywords"] != null)
                    {
                        JArray ja = (JArray)spiderMeta["keywords"];
                        string temp = ja[0].ToString();
                        for (int i = 1; i < ja.Count; i++)
                        {
                            temp += "," + ja[i];
                        }
                        sb.AddColumn("spider_words", escape(temp));
                    }
                    if (spiderMeta["play_count"] != null)
                    {
                        sb.AddColumn("play_count", spiderMeta["play_count"].ToString());
                    }
                    if (spiderMeta["spider_time"] != null)
                    {
                        DateTime dt;
                        if (DateTime.TryParse(spiderMeta["spider_time"].ToString(), out dt))
                        {
                            sb.AddColumn("spider_time", dt.ToString("yyyy-MM-dd"));
                        }                        
                    }
                    if (spiderMeta["site_name"] != null)
                    {
                        sb.AddColumn("site_name", escape(spiderMeta["site_name"].ToString()));
                    }
                    if (spiderMeta["site_name_cn"] != null)
                    {
                        sb.AddColumn("site_name_cn", escape(spiderMeta["site_name_cn"].ToString()));
                    }
                    if (spiderMeta["info"] != null)
                    {
                        sb.AddColumn("info", escape(spiderMeta["info"].ToString()));
                    }
                    if (spiderMeta["info_cn"] != null)
                    {
                        sb.AddColumn("info_cn", escape(spiderMeta["info_cn"].ToString()));
                    }
                    if (spiderMeta["url"] != null)
                    {
                        sb.AddColumn("url", escape(spiderMeta["url"].ToString()));
                    }
                    if (spiderMeta["upload_time"] != null)
                    {
                        DateTime dt;
                        if (DateTime.TryParse(spiderMeta["upload_time"].ToString(), out dt))
                        {
                            sb.AddColumn("upload_time", dt.ToString("yyyy-MM-dd"));
                        } 
                    }
                    if (spiderMeta["title"] != null)
                    {
                        sb.AddColumn("title", escape(spiderMeta["title"].ToString()));
                    }
                    if (spiderMeta["title_cn"] != null)
                    {
                        sb.AddColumn("title_cn", escape(spiderMeta["title_cn"].ToString()));
                    }
//                     if (spiderMeta["task_id"] != null)
//                     {
//                         sb.AddColumn("task_id", escape(spiderMeta["task_id"].ToString()));
//                     }
                }

                if (bSuccess)
                {
                    sb.AddColumn("hdfs_address", escape(info));
                }
                else
                {
                    sb.AddColumn("error_message", escape(info));
                } 
            }
            catch (System.Exception ex)
            {
                string allMsg="";
                if (!bSuccess)
                {
                    allMsg = info;
                }
                errMsg = ex.Message;
                allMsg += ";" + errMsg;
                sb.AddColumn("error_message", escape(allMsg));
                bMysqlSuccess = false;
            }
            
            //write metadata to mysql
            sql = sb.GetInsertSQL();
            MySqlCommand cmd = new MySqlCommand(sql, m_mysqlConn);
            int code = cmd.ExecuteNonQuery();

            if (m_log != null)
            {
                m_log.printLog(DateTime.Now.ToString() + " 保存视频元数据信息到MYSQL数据库完成");
                if (errMsg != "")
                {
                    m_log.printLog("错误：" + errMsg);
                }                
                m_log.printLog("");
            }
            message = errMsg;
            return bMysqlSuccess;
        }

        //mysql转义字符串
        public string escape(string oldstr)
        {
            return oldstr.Replace("\'", "\\\'").Replace("\"", "\\\"");
        }

        public WebResponse uploadCorpus2CalculateNode(string strFile)
        {
            string strUploadURL = m_calServerUrl + "/upload_corpus";
            return httpUploadFile(strUploadURL, strFile, "file", "application/octet-stream");
        }

        public WebResponse httpGET(string url, int timeout=-1)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            if (timeout != -1)
            {
                request.Timeout = timeout;
            }            
            request.Method = "GET";
            request.KeepAlive = true;
            if (m_cookies != null)
            {
                request.CookieContainer = m_cookies;
            }

            try
            {
                WebResponse response = request.GetResponse();
                if (m_bSaveSession)
                {
                    m_cookies = request.CookieContainer;
                    m_bSaveSession = false;
                }
                return response;
            }
            catch (Exception ex)
            {
                if (m_log != null)
                {
                    m_log.printLog("HTTP请求错误：" + ex.Message);
                    m_log.printLog("");
                }
                return null;
            }   
        }

        public WebResponse httpGET(string url, out string message, int timeout = -1)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            if (timeout != -1)
            {
                //request.Timeout = timeout;
            }
            request.Method = "GET";
            request.KeepAlive = true;
            if (m_cookies != null)
            {
                request.CookieContainer = m_cookies;
            }
            WebResponse response=null;
            try
            {
                response = request.GetResponse();
                if (m_bSaveSession)
                {
                    m_cookies = request.CookieContainer;
                    m_bSaveSession = false;
                }
                message = "";
                return response;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                if (response!=null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
                return null;
            } 
        }


        public WebResponse httpGet2CalculateNode(string param,int timeout=-1)
        {
            string url = m_calServerUrl + param;
            return httpGET(url, timeout);         
        }

        public static bool SetAllowUnsafeHeaderParsing(bool useUnsafe)
        {
            //Get the assembly that contains the internal class
            System.Reflection.Assembly aNetAssembly = System.Reflection.Assembly.GetAssembly(typeof(System.Net.Configuration.SettingsSection));
            if (aNetAssembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type aSettingsType = aNetAssembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (aSettingsType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created allready the property will create it for us.
                    object anInstance = aSettingsType.InvokeMember("Section",
                      System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.NonPublic, null, null, new object[] { });

                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework is unsafe header parsing should be allowed or not
                        System.Reflection.FieldInfo aUseUnsafeHeaderParsing = aSettingsType.GetField("useUnsafeHeaderParsing", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, useUnsafe);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void saveSession()
        {
            m_bSaveSession = true;
            m_cookies = new CookieContainer();
        }

        public void removeSession()
        {
            m_bSaveSession = false;
            m_cookies = null;
        }

        public int mysqlNonQuery(string sql)
        {
            try
            {
                m_mysqlConn.Open();
            }
            catch (System.Exception ex)
            {
                m_log.printLog(ex.Message);
            }
            MySqlCommand cmd = new MySqlCommand(sql, m_mysqlConn);
            int code = cmd.ExecuteNonQuery();
            m_mysqlConn.Close();
            return code;
            
        }

        public MySqlDataReader mysqlQuery(string sql)
        {
            //创建一个MySqlCommand对象
            MySqlCommand cmd = new MySqlCommand(sql,m_mysqlConn);
            MySqlDataReader data = cmd.ExecuteReader();
            //m_mysqlConn.Close();
            return data; 
        }

        public void mysqlClose()
        {
            if (m_mysqlConn.State == System.Data.ConnectionState.Open)
            {
                m_mysqlConn.Close();
            }            
        }

        public void mysqlOpen()
        {
            try
            {
                m_mysqlConn.Open();
            }
            catch (System.Exception ex)
            {
                m_log.printLog(ex.Message);
            }
        }
    }
}

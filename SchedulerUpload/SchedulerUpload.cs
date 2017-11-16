using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.TaskScheduler;
using System.IO;

namespace VisTool
{
    class SchedulerUpload : ILog
    {
        StreamWriter m_sw;

        serverConnector m_serverConnector;

        string m_folderPath;

        string m_modelName;

        ~SchedulerUpload()
        {
            if (m_sw != null)
            {
                m_sw.Close();
            }
        }

        public void printLog(string str)
        {
            str += "\n";
            m_sw.WriteLine(str);
        }

        void init(string[] args)
        {
            //init parameters
            if (args.GetLength(0) != 3)
            {
                return;
            }
            
            m_serverConnector = new serverConnector(args[0], this);
            m_folderPath = args[1];
            m_modelName = args[2];
            m_serverConnector.saveSession();
            //create a streamwriter to write log
            string strLogPath = Directory.GetCurrentDirectory() + "\\log";
            if (!Directory.Exists(strLogPath))
            {
                Directory.CreateDirectory(strLogPath);
            }

            string strLogName = DateTime.Now.ToLongDateString() + "_Scheduler.txt";

            string strFullName = strLogPath + "\\" + strLogName;

            if (!File.Exists(strFullName))
            {
                File.Create(strFullName).Close();
            }            
            m_sw = new StreamWriter(strFullName, true);
        }

        void upload()
        {
            m_serverConnector.updateVideoDatabase(m_folderPath, m_modelName);
        }

        static void Main(string[] args)
        {
            Console.WriteLine(args[0]);
            Console.WriteLine(args[1]);
            Console.WriteLine(args[2]);
            SchedulerUpload myPro = new SchedulerUpload();
            myPro.init(args);
            myPro.upload();
        }
    }
}

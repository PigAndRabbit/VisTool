using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace VisTool
{
    public class MysqlHandler
    {
        MySqlConnection m_conn;

        public void Connect(string conn)
        {
            m_conn = new MySqlConnection(conn);            
        }

        public void Close()
        {
            m_conn.Close();
        }

        public int ExecuteNonQuery(string sql)
        {
            int code = -1;
            try
            {
                m_conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, m_conn);
                code = cmd.ExecuteNonQuery();
                m_conn.Close();
                return code;
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
                return code;
            }
           
        }
        
    }
}

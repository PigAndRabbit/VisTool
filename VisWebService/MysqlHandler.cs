using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace VisTool
{
    public class sqlBuilder
    {
        public void AddColumn(string name, string value)
        {
            m_pair.Add(name, value);
        }

        public void AddCondition(string name, string value)
        {
            m_conditionColumn = name;
            m_conditionValue = value;
        }

        public void SetTable(string tablename)
        {
            m_tablename = tablename;
        }

        public string GetInsertSQL()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO `");
            sqlBuilder.Append(m_tablename);
            sqlBuilder.Append("` ");
            sqlBuilder.Append(getColNames());
            sqlBuilder.Append(" VALUES");
            sqlBuilder.Append(getColValues());
            return sqlBuilder.ToString();
        }

        public string getUpdateSQL()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE `");
            sqlBuilder.Append(m_tablename);
            sqlBuilder.Append("` SET ");
            bool bFirst = true;
            foreach (var item in m_pair)
            {
                if (bFirst)
                {
                    sqlBuilder.Append(item.Key + "='" + item.Value + "'");
                    bFirst = false;
                }
                else
                {
                    sqlBuilder.Append("," + item.Key + "='" + item.Value + "'");
                }                
            }
            sqlBuilder.Append(" WHERE " + m_conditionColumn + "='" + m_conditionValue + "'");
            return sqlBuilder.ToString();
        }

        string getColNames()
        {
            StringBuilder strBuilder = new StringBuilder();
            
            bool bFirst = true;
            foreach (string key in m_pair.Keys)
            {
                if (bFirst)
                {
                    strBuilder.Append("(`");
                    bFirst = false;
                }
                else
                {
                    strBuilder.Append("`,`");
                }                
                strBuilder.Append(key);                
            }
            strBuilder.Append("`)");
            return strBuilder.ToString();
        }

        string getColValues()
        {
            StringBuilder strBuilder = new StringBuilder();

            bool bFirst = true;
            foreach (string value in m_pair.Values)
            {
                if (bFirst)
                {
                    strBuilder.Append("('");
                    bFirst = false;
                }
                else
                {
                    strBuilder.Append("','");
                }
                strBuilder.Append(value);
            }
            strBuilder.Append("')");
            return strBuilder.ToString();
        }

        string m_tablename;

        Dictionary<string, string> m_pair = new Dictionary<string, string>();
        string m_conditionColumn;
        string m_conditionValue;
    }

}

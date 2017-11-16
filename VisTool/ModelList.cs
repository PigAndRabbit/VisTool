using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;

namespace VisTool
{
    public partial class ModelList : Form
    {
        public string m_selectedModelName;
        DataUploadFrm m_parent;
        public ModelList(Form parent)
        {
            m_parent = (DataUploadFrm)parent;
            InitializeComponent();
        }

        private void ModelList_Load(object sender, EventArgs e)
        {
            //基本设置
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            string sql = "Select * FROM `models`";
            m_parent.m_serverConnector.mysqlOpen();
            MySqlDataReader dr = m_parent.m_serverConnector.mysqlQuery(sql);
            for (int i = 0; i < dr.FieldCount; i++)
            {
                dataGridView1.Columns.Add(dr.GetName(i), dr.GetName(i));                
            }
            while (dr.Read())
            {
                int index = dataGridView1.Rows.Add();
                for(int i=0;i<dr.FieldCount;i++)
                {
                    dataGridView1.Rows[index].Cells[i].Value = dr[i].ToString();
                }
            }

            m_parent.m_serverConnector.mysqlClose();

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_selectedModelName = dataGridView1["name", dataGridView1.CurrentCell.RowIndex].Value.ToString();
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void mList_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows != null)
            {
                m_selectedModelName = dataGridView1["name", dataGridView1.CurrentCell.RowIndex].Value.ToString();
                this.DialogResult = DialogResult.OK;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisTool
{
    public partial class HDFSDirFrm : Form
    {
        public string HDFSPath;

        public HDFSDirFrm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (TBHDFSPath.Text == "")
            {
                MessageBox.Show("请输入HDFS上的文件夹路径");                
            }
            else
            {
                HDFSPath = TBHDFSPath.Text;
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}

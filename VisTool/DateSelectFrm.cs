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
    public enum MissionType
    {
        PerDay,
        PerWeek,
        PerMonth,
    };

    public partial class DateSelectFrm : Form
    {
        
        public DateTime m_dtStart;
        public DateTime m_dtEnd;
        public DateTime m_dtTime;
        public MissionType m_eType;
        public bool m_bStartNow;

        public DateSelectFrm()
        {
            InitializeComponent();
        }

        private void DailyUploadFrm_Load(object sender, EventArgs e)
        {
            //控制日期或时间的显示格式

            this.dateTimePicker1.CustomFormat = "yyyy年MM月dd日";

            this.dateTimePicker1.Format = DateTimePickerFormat.Custom;

            this.dateTimePicker2.CustomFormat = "yyyy年MM月dd日";

            this.dateTimePicker2.Format = DateTimePickerFormat.Custom;

            this.dateTimePicker2.Value = DateTime.Now.AddDays(1);

            this.dateTimePicker3.ShowUpDown = true;

            this.dateTimePicker3.CustomFormat = "HH:mm:ss";

            this.dateTimePicker3.Format = DateTimePickerFormat.Custom;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_dtStart = Convert.ToDateTime(dateTimePicker1.Value.ToShortDateString() + " " + dateTimePicker3.Value.ToLongTimeString());
            m_dtEnd = Convert.ToDateTime(dateTimePicker2.Value.ToShortDateString() + " " + dateTimePicker3.Value.ToLongTimeString());

            if (DateTime.Compare(m_dtStart, m_dtEnd)>0)
            {
                MessageBox.Show("开始日期应小于结束日期！", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_bStartNow = checkBox1.Checked;

            if (radioButton1.Checked)
            {
                m_eType = MissionType.PerDay;
            }
            else if (radioButton2.Checked)
            {
                m_eType = MissionType.PerWeek;
            }
            else if (radioButton3.Checked)
            {
                m_eType = MissionType.PerMonth;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                dateTimePicker3.Enabled = false;
                dateTimePicker3.Value = DateTime.Now;
                dateTimePicker1.Value = DateTime.Now;
            }
            else
            {
                dateTimePicker3.Enabled = true;
            }
        }
    }
}

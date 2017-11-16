namespace VisTool
{
    partial class ConnFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mUserTB = new System.Windows.Forms.TextBox();
            this.mPwdTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.mPortTB = new System.Windows.Forms.TextBox();
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mIPTB = new System.Windows.Forms.TextBox();
            this.tbCode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbDatabase = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbHDFSNodes = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbHDFSAdd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbCalAdd = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbGraph = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "数据库用户名";
            // 
            // mUserTB
            // 
            this.mUserTB.Location = new System.Drawing.Point(90, 52);
            this.mUserTB.Name = "mUserTB";
            this.mUserTB.Size = new System.Drawing.Size(208, 21);
            this.mUserTB.TabIndex = 3;
            this.mUserTB.Text = "root";
            // 
            // mPwdTB
            // 
            this.mPwdTB.Location = new System.Drawing.Point(90, 85);
            this.mPwdTB.Name = "mPwdTB";
            this.mPwdTB.PasswordChar = '●';
            this.mPwdTB.Size = new System.Drawing.Size(208, 21);
            this.mPwdTB.TabIndex = 4;
            this.mPwdTB.Text = "123456";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(199, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "端口号";
            // 
            // mPortTB
            // 
            this.mPortTB.Location = new System.Drawing.Point(246, 20);
            this.mPortTB.Name = "mPortTB";
            this.mPortTB.Size = new System.Drawing.Size(52, 21);
            this.mPortTB.TabIndex = 8;
            this.mPortTB.Text = "3306";
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(151, 424);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 9;
            this.butOK.Text = "确定";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(232, 424);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 10;
            this.butCancel.Text = "取消";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(70, 424);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "测试连接";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mIPTB);
            this.groupBox1.Controls.Add(this.tbCode);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbDatabase);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.mUserTB);
            this.groupBox1.Controls.Add(this.mPwdTB);
            this.groupBox1.Controls.Add(this.mPortTB);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(306, 179);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MySQL连接";
            // 
            // mIPTB
            // 
            this.mIPTB.Location = new System.Drawing.Point(90, 20);
            this.mIPTB.Name = "mIPTB";
            this.mIPTB.Size = new System.Drawing.Size(103, 21);
            this.mIPTB.TabIndex = 13;
            this.mIPTB.Text = "127.0.0.1";
            // 
            // tbCode
            // 
            this.tbCode.Location = new System.Drawing.Point(90, 149);
            this.tbCode.Name = "tbCode";
            this.tbCode.Size = new System.Drawing.Size(208, 21);
            this.tbCode.TabIndex = 12;
            this.tbCode.Text = "utf8";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 152);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "数据库编码";
            // 
            // tbDatabase
            // 
            this.tbDatabase.Location = new System.Drawing.Point(90, 117);
            this.tbDatabase.Name = "tbDatabase";
            this.tbDatabase.Size = new System.Drawing.Size(208, 21);
            this.tbDatabase.TabIndex = 10;
            this.tbDatabase.Text = "visdatabase";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "数据库名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "数据库密码";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbHDFSNodes);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.tbHDFSAdd);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 194);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(306, 112);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "HDFS连接";
            // 
            // tbHDFSNodes
            // 
            this.tbHDFSNodes.Location = new System.Drawing.Point(90, 51);
            this.tbHDFSNodes.Multiline = true;
            this.tbHDFSNodes.Name = "tbHDFSNodes";
            this.tbHDFSNodes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbHDFSNodes.Size = new System.Drawing.Size(208, 55);
            this.tbHDFSNodes.TabIndex = 15;
            this.tbHDFSNodes.Text = "master:10.168.103.104\r\nslave:10.168.103.105";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "服务器节点";
            // 
            // tbHDFSAdd
            // 
            this.tbHDFSAdd.Location = new System.Drawing.Point(90, 23);
            this.tbHDFSAdd.Name = "tbHDFSAdd";
            this.tbHDFSAdd.Size = new System.Drawing.Size(208, 21);
            this.tbHDFSAdd.TabIndex = 13;
            this.tbHDFSAdd.Text = "http://10.168.103.106:50070/webhdfs/v1/";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "服务器地址";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbCalAdd);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(12, 306);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(306, 55);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "计算节点";
            // 
            // tbCalAdd
            // 
            this.tbCalAdd.Location = new System.Drawing.Point(90, 23);
            this.tbCalAdd.Name = "tbCalAdd";
            this.tbCalAdd.Size = new System.Drawing.Size(208, 21);
            this.tbCalAdd.TabIndex = 13;
            this.tbCalAdd.Text = "http://10.168.103.17:5000";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "服务器地址";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbGraph);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Location = new System.Drawing.Point(12, 363);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(306, 55);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "图数据库";
            // 
            // tbGraph
            // 
            this.tbGraph.Location = new System.Drawing.Point(90, 23);
            this.tbGraph.Name = "tbGraph";
            this.tbGraph.Size = new System.Drawing.Size(208, 21);
            this.tbGraph.TabIndex = 13;
            this.tbGraph.Text = "http://10.168.103.101:8000";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "服务器地址";
            // 
            // ConnFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(330, 471);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnFrm";
            this.Text = "连接配置";
            this.Load += new System.EventHandler(this.MySQlConn_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox mUserTB;
        private System.Windows.Forms.TextBox mPwdTB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox mPortTB;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbDatabase;
        private System.Windows.Forms.TextBox tbHDFSNodes;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbHDFSAdd;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbCalAdd;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox mIPTB;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tbGraph;
        private System.Windows.Forms.Label label9;
    }
}
namespace VisTool
{
    partial class MainUI
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.数据标注ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.训练ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cNNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sVNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.存储ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.预测ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.连接设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar,
            this.statusInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 507);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(855, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // statusInfo
            // 
            this.statusInfo.Name = "statusInfo";
            this.statusInfo.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.数据标注ToolStripMenuItem,
            this.训练ToolStripMenuItem,
            this.存储ToolStripMenuItem,
            this.预测ToolStripMenuItem,
            this.连接设置ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(855, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 数据标注ToolStripMenuItem
            // 
            this.数据标注ToolStripMenuItem.Name = "数据标注ToolStripMenuItem";
            this.数据标注ToolStripMenuItem.Size = new System.Drawing.Size(56, 21);
            this.数据标注ToolStripMenuItem.Text = "语料库";
            this.数据标注ToolStripMenuItem.Click += new System.EventHandler(this.数据标注ToolStripMenuItem_Click);
            // 
            // 训练ToolStripMenuItem
            // 
            this.训练ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cNNToolStripMenuItem,
            this.sVNToolStripMenuItem});
            this.训练ToolStripMenuItem.Name = "训练ToolStripMenuItem";
            this.训练ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.训练ToolStripMenuItem.Text = "训练";
            // 
            // cNNToolStripMenuItem
            // 
            this.cNNToolStripMenuItem.Name = "cNNToolStripMenuItem";
            this.cNNToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
            this.cNNToolStripMenuItem.Text = "CNN";
            this.cNNToolStripMenuItem.Click += new System.EventHandler(this.cNNToolStripMenuItem_Click);
            // 
            // sVNToolStripMenuItem
            // 
            this.sVNToolStripMenuItem.Name = "sVNToolStripMenuItem";
            this.sVNToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
            this.sVNToolStripMenuItem.Text = "SVM";
            this.sVNToolStripMenuItem.Click += new System.EventHandler(this.sVNToolStripMenuItem_Click);
            // 
            // 存储ToolStripMenuItem
            // 
            this.存储ToolStripMenuItem.Name = "存储ToolStripMenuItem";
            this.存储ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.存储ToolStripMenuItem.Text = "存储";
            this.存储ToolStripMenuItem.Click += new System.EventHandler(this.存储ToolStripMenuItem_Click);
            // 
            // 预测ToolStripMenuItem
            // 
            this.预测ToolStripMenuItem.Name = "预测ToolStripMenuItem";
            this.预测ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.预测ToolStripMenuItem.Text = "数据上传";
            this.预测ToolStripMenuItem.Click += new System.EventHandler(this.预测ToolStripMenuItem_Click);
            // 
            // 连接设置ToolStripMenuItem
            // 
            this.连接设置ToolStripMenuItem.Name = "连接设置ToolStripMenuItem";
            this.连接设置ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.连接设置ToolStripMenuItem.Text = "连接设置";
            this.连接设置ToolStripMenuItem.Click += new System.EventHandler(this.连接设置ToolStripMenuItem_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.AllowNavigation = false;
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser1.Location = new System.Drawing.Point(0, 25);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(855, 482);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.WebBrowserShortcutsEnabled = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // MainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 529);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainUI";
            this.Text = "可视化训练工具";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainUI_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 数据标注ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 训练ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cNNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sVNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 存储ToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ToolStripMenuItem 预测ToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel statusInfo;
        private System.Windows.Forms.ToolStripMenuItem 连接设置ToolStripMenuItem;

    }
}


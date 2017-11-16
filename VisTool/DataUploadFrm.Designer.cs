namespace VisTool
{
    partial class DataUploadFrm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataUploadFrm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.选择模型ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upload = new System.Windows.Forms.ToolStripMenuItem();
            this.timmingUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.开始ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.directoryTree = new System.Windows.Forms.TreeView();
            this.directoryIcons = new System.Windows.Forms.ImageList(this.components);
            this.logInfo = new System.Windows.Forms.TextBox();
            this.filesIcons = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选择模型ToolStripMenuItem,
            this.upload,
            this.timmingUpload});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(970, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 选择模型ToolStripMenuItem
            // 
            this.选择模型ToolStripMenuItem.Name = "选择模型ToolStripMenuItem";
            this.选择模型ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.选择模型ToolStripMenuItem.Text = "选择模型";
            this.选择模型ToolStripMenuItem.Click += new System.EventHandler(this.选择模型ToolStripMenuItem_Click);
            // 
            // upload
            // 
            this.upload.Name = "upload";
            this.upload.Size = new System.Drawing.Size(44, 21);
            this.upload.Text = "上传";
            this.upload.Click += new System.EventHandler(this.上传ToolStripMenuItem_Click);
            // 
            // timmingUpload
            // 
            this.timmingUpload.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始ToolStripMenuItem,
            this.停止ToolStripMenuItem});
            this.timmingUpload.Name = "timmingUpload";
            this.timmingUpload.Size = new System.Drawing.Size(68, 21);
            this.timmingUpload.Text = "定时上传";
            // 
            // 开始ToolStripMenuItem
            // 
            this.开始ToolStripMenuItem.Name = "开始ToolStripMenuItem";
            this.开始ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.开始ToolStripMenuItem.Text = "创建";
            this.开始ToolStripMenuItem.Click += new System.EventHandler(this.创建定时上传ToolStripMenuItem_Click);
            // 
            // 停止ToolStripMenuItem
            // 
            this.停止ToolStripMenuItem.Name = "停止ToolStripMenuItem";
            this.停止ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.停止ToolStripMenuItem.Text = "停止";
            this.停止ToolStripMenuItem.Click += new System.EventHandler(this.停止ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 530);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(970, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.directoryTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.logInfo);
            this.splitContainer1.Size = new System.Drawing.Size(970, 505);
            this.splitContainer1.SplitterDistance = 263;
            this.splitContainer1.TabIndex = 2;
            // 
            // directoryTree
            // 
            this.directoryTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directoryTree.ImageIndex = 0;
            this.directoryTree.ImageList = this.directoryIcons;
            this.directoryTree.Location = new System.Drawing.Point(0, 0);
            this.directoryTree.Name = "directoryTree";
            this.directoryTree.SelectedImageIndex = 0;
            this.directoryTree.Size = new System.Drawing.Size(263, 505);
            this.directoryTree.TabIndex = 0;
            this.directoryTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.directoryTree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.directoryTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.directoryTree_AfterSelect);
            this.directoryTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.directoryTree_NodeMouseClick);
            this.directoryTree.Click += new System.EventHandler(this.directoryTree_Click);
            // 
            // directoryIcons
            // 
            this.directoryIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("directoryIcons.ImageStream")));
            this.directoryIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.directoryIcons.Images.SetKeyName(0, "20080616225145414[1].png");
            this.directoryIcons.Images.SetKeyName(1, "20080616214517520[1].png");
            this.directoryIcons.Images.SetKeyName(2, "20080616214517528[1].png");
            this.directoryIcons.Images.SetKeyName(3, "20080616225145459[1].png");
            this.directoryIcons.Images.SetKeyName(4, "20080616225145488[1].png");
            this.directoryIcons.Images.SetKeyName(5, "20080616213743305[1].png");
            // 
            // logInfo
            // 
            this.logInfo.BackColor = System.Drawing.SystemColors.InfoText;
            this.logInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logInfo.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.logInfo.ForeColor = System.Drawing.SystemColors.Info;
            this.logInfo.Location = new System.Drawing.Point(0, 0);
            this.logInfo.Multiline = true;
            this.logInfo.Name = "logInfo";
            this.logInfo.Size = new System.Drawing.Size(703, 505);
            this.logInfo.TabIndex = 0;
            // 
            // filesIcons
            // 
            this.filesIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("filesIcons.ImageStream")));
            this.filesIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.filesIcons.Images.SetKeyName(0, "20080616214517520[1].png");
            // 
            // DataUploadFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 552);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DataUploadFrm";
            this.Text = "数据上传";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataUploadFrm_FormClosing);
            this.Load += new System.EventHandler(this.DataUploadFrm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripMenuItem upload;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView directoryTree;
        private System.Windows.Forms.ImageList directoryIcons;
        private System.Windows.Forms.ImageList filesIcons;
        private System.Windows.Forms.TextBox logInfo;
        private System.Windows.Forms.ToolStripMenuItem timmingUpload;
        private System.Windows.Forms.ToolStripMenuItem 选择模型ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始ToolStripMenuItem;
    }
}
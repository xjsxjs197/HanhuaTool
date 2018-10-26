namespace Hanhua.Common
{
    partial class StartMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartMenu));
            this.btnFntTool = new System.Windows.Forms.Button();
            this.btnTxtTool = new System.Windows.Forms.Button();
            this.commandPanel = new System.Windows.Forms.Panel();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnFileEdit = new System.Windows.Forms.Button();
            this.btnNgcIso = new System.Windows.Forms.Button();
            this.btnImgTool = new System.Windows.Forms.Button();
            this.btnAutoBuild = new System.Windows.Forms.Button();
            this.commandPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Size = new System.Drawing.Size(273, 156);
            // 
            // btnFntTool
            // 
            this.btnFntTool.Location = new System.Drawing.Point(14, 11);
            this.btnFntTool.Name = "btnFntTool";
            this.btnFntTool.Size = new System.Drawing.Size(112, 29);
            this.btnFntTool.TabIndex = 0;
            this.btnFntTool.Text = "字库处理工具";
            this.btnFntTool.UseVisualStyleBackColor = true;
            this.btnFntTool.Click += new System.EventHandler(this.btnFntTool_Click);
            // 
            // btnTxtTool
            // 
            this.btnTxtTool.Location = new System.Drawing.Point(14, 46);
            this.btnTxtTool.Name = "btnTxtTool";
            this.btnTxtTool.Size = new System.Drawing.Size(112, 29);
            this.btnTxtTool.TabIndex = 1;
            this.btnTxtTool.Text = "文本处理工具";
            this.btnTxtTool.UseVisualStyleBackColor = true;
            this.btnTxtTool.Click += new System.EventHandler(this.btnTxtTool_Click);
            // 
            // commandPanel
            // 
            this.commandPanel.Controls.Add(this.btnAutoBuild);
            this.commandPanel.Controls.Add(this.btnTest);
            this.commandPanel.Controls.Add(this.btnFileEdit);
            this.commandPanel.Controls.Add(this.btnNgcIso);
            this.commandPanel.Controls.Add(this.btnImgTool);
            this.commandPanel.Controls.Add(this.btnTxtTool);
            this.commandPanel.Controls.Add(this.btnFntTool);
            this.commandPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandPanel.Location = new System.Drawing.Point(0, 0);
            this.commandPanel.Name = "commandPanel";
            this.commandPanel.Size = new System.Drawing.Size(273, 156);
            this.commandPanel.TabIndex = 7;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(141, 81);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(112, 29);
            this.btnTest.TabIndex = 25;
            this.btnTest.Text = "测  试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnFileEdit
            // 
            this.btnFileEdit.Location = new System.Drawing.Point(141, 11);
            this.btnFileEdit.Name = "btnFileEdit";
            this.btnFileEdit.Size = new System.Drawing.Size(112, 29);
            this.btnFileEdit.TabIndex = 24;
            this.btnFileEdit.Text = "文件处理工具";
            this.btnFileEdit.UseVisualStyleBackColor = true;
            this.btnFileEdit.Click += new System.EventHandler(this.btnFileEdit_Click);
            // 
            // btnNgcIso
            // 
            this.btnNgcIso.Location = new System.Drawing.Point(14, 81);
            this.btnNgcIso.Name = "btnNgcIso";
            this.btnNgcIso.Size = new System.Drawing.Size(112, 29);
            this.btnNgcIso.TabIndex = 23;
            this.btnNgcIso.Text = "打补丁工具";
            this.btnNgcIso.UseVisualStyleBackColor = true;
            this.btnNgcIso.Click += new System.EventHandler(this.btnNgcIso_Click);
            // 
            // btnImgTool
            // 
            this.btnImgTool.Location = new System.Drawing.Point(141, 46);
            this.btnImgTool.Name = "btnImgTool";
            this.btnImgTool.Size = new System.Drawing.Size(112, 29);
            this.btnImgTool.TabIndex = 15;
            this.btnImgTool.Text = "图片处理工具";
            this.btnImgTool.UseVisualStyleBackColor = true;
            this.btnImgTool.Click += new System.EventHandler(this.btnImgTool_Click);
            // 
            // btnAutoBuild
            // 
            this.btnAutoBuild.Location = new System.Drawing.Point(14, 116);
            this.btnAutoBuild.Name = "btnAutoBuild";
            this.btnAutoBuild.Size = new System.Drawing.Size(112, 29);
            this.btnAutoBuild.TabIndex = 26;
            this.btnAutoBuild.Text = "编译Retroarch";
            this.btnAutoBuild.UseVisualStyleBackColor = true;
            this.btnAutoBuild.Click += new System.EventHandler(this.btnAutoBuild_Click);
            // 
            // StartMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 181);
            this.Controls.Add(this.commandPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(279, 28);
            this.Name = "StartMenu";
            this.Text = "Wii/Ngc 汉化入口";
            this.Controls.SetChildIndex(this.pnlTopBody, 0);
            this.Controls.SetChildIndex(this.commandPanel, 0);
            this.commandPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFntTool;
        private System.Windows.Forms.Button btnTxtTool;
        private System.Windows.Forms.Panel commandPanel;
        private System.Windows.Forms.Button btnImgTool;
        private System.Windows.Forms.Button btnNgcIso;
        private System.Windows.Forms.Button btnFileEdit;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnAutoBuild;
    }
}
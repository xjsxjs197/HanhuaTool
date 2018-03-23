namespace Hanhua.TextEditTools.BioCvEdit
{
    partial class BioCvAfsEditor
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
            this.pnlButton = new System.Windows.Forms.Panel();
            this.btnCreateAfs = new System.Windows.Forms.Button();
            this.btnSelFolder = new System.Windows.Forms.Button();
            this.btnSelFile = new System.Windows.Forms.Button();
            this.pnlToolBody = new System.Windows.Forms.Panel();
            this.lstAsfFiles = new System.Windows.Forms.ListBox();
            this.pnlTopBody.SuspendLayout();
            this.pnlButton.SuspendLayout();
            this.pnlToolBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Controls.Add(this.pnlToolBody);
            this.pnlTopBody.Controls.Add(this.pnlButton);
            this.pnlTopBody.Size = new System.Drawing.Size(418, 504);
            // 
            // pnlButton
            // 
            this.pnlButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.pnlButton.Controls.Add(this.btnCreateAfs);
            this.pnlButton.Controls.Add(this.btnSelFolder);
            this.pnlButton.Controls.Add(this.btnSelFile);
            this.pnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButton.Location = new System.Drawing.Point(0, 467);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.Size = new System.Drawing.Size(418, 37);
            this.pnlButton.TabIndex = 0;
            // 
            // btnCreateAfs
            // 
            this.btnCreateAfs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateAfs.Location = new System.Drawing.Point(102, 6);
            this.btnCreateAfs.Name = "btnCreateAfs";
            this.btnCreateAfs.Size = new System.Drawing.Size(79, 26);
            this.btnCreateAfs.TabIndex = 2;
            this.btnCreateAfs.Text = "生成文件";
            this.btnCreateAfs.UseVisualStyleBackColor = true;
            this.btnCreateAfs.Click += new System.EventHandler(this.btnCreateAfs_Click);
            // 
            // btnSelFolder
            // 
            this.btnSelFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelFolder.Location = new System.Drawing.Point(227, 6);
            this.btnSelFolder.Name = "btnSelFolder";
            this.btnSelFolder.Size = new System.Drawing.Size(79, 26);
            this.btnSelFolder.TabIndex = 1;
            this.btnSelFolder.Text = "选择目录";
            this.btnSelFolder.UseVisualStyleBackColor = true;
            this.btnSelFolder.Click += new System.EventHandler(this.btnSelFolder_Click);
            // 
            // btnSelFile
            // 
            this.btnSelFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelFile.Location = new System.Drawing.Point(321, 6);
            this.btnSelFile.Name = "btnSelFile";
            this.btnSelFile.Size = new System.Drawing.Size(79, 26);
            this.btnSelFile.TabIndex = 0;
            this.btnSelFile.Text = "选择文件";
            this.btnSelFile.UseVisualStyleBackColor = true;
            this.btnSelFile.Click += new System.EventHandler(this.btnSelFile_Click);
            // 
            // pnlToolBody
            // 
            this.pnlToolBody.Controls.Add(this.lstAsfFiles);
            this.pnlToolBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlToolBody.Location = new System.Drawing.Point(0, 0);
            this.pnlToolBody.Margin = new System.Windows.Forms.Padding(0);
            this.pnlToolBody.Name = "pnlToolBody";
            this.pnlToolBody.Size = new System.Drawing.Size(418, 467);
            this.pnlToolBody.TabIndex = 1;
            // 
            // lstAsfFiles
            // 
            this.lstAsfFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstAsfFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAsfFiles.FormattingEnabled = true;
            this.lstAsfFiles.ItemHeight = 12;
            this.lstAsfFiles.Location = new System.Drawing.Point(0, 0);
            this.lstAsfFiles.Margin = new System.Windows.Forms.Padding(0);
            this.lstAsfFiles.Name = "lstAsfFiles";
            this.lstAsfFiles.Size = new System.Drawing.Size(418, 458);
            this.lstAsfFiles.TabIndex = 0;
            this.lstAsfFiles.SelectedIndexChanged += new System.EventHandler(this.lstAsfFiles_SelectedIndexChanged);
            // 
            // BioCvAfsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 529);
            this.Name = "BioCvAfsEditor";
            this.Text = "BioCvAfsEditor";
            this.pnlTopBody.ResumeLayout(false);
            this.pnlButton.ResumeLayout(false);
            this.pnlToolBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlToolBody;
        private System.Windows.Forms.Panel pnlButton;
        private System.Windows.Forms.ListBox lstAsfFiles;
        private System.Windows.Forms.Button btnSelFile;
        private System.Windows.Forms.Button btnSelFolder;
        private System.Windows.Forms.Button btnCreateAfs;
    }
}
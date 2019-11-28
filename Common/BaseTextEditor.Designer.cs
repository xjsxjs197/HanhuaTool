
namespace Hanhua.Common
{
    partial class BaseTextEditor
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
            this.pnlCommand = new System.Windows.Forms.Panel();
            this.pnlDisk = new System.Windows.Forms.Panel();
            this.rdoBDisk = new System.Windows.Forms.RadioButton();
            this.rdoADisk = new System.Windows.Forms.RadioButton();
            this.btnReLoad = new System.Windows.Forms.Button();
            this.btnPatch = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlEditor = new System.Windows.Forms.Panel();
            this.txtJp = new Hanhua.Common.RichTextBoxEx();
            this.txtCn = new Hanhua.Common.RichTextBoxEx();
            this.fileList = new System.Windows.Forms.ListBox();
            this.pnlCommand.SuspendLayout();
            this.pnlDisk.SuspendLayout();
            this.pnlEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Size = new System.Drawing.Size(912, 579);
            // 
            // pnlCommand
            // 
            this.pnlCommand.BackColor = System.Drawing.Color.SandyBrown;
            this.pnlCommand.Controls.Add(this.pnlDisk);
            this.pnlCommand.Controls.Add(this.btnReLoad);
            this.pnlCommand.Controls.Add(this.btnPatch);
            this.pnlCommand.Controls.Add(this.btnImport);
            this.pnlCommand.Controls.Add(this.btnExport);
            this.pnlCommand.Controls.Add(this.btnSave);
            this.pnlCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCommand.Location = new System.Drawing.Point(0, 545);
            this.pnlCommand.Name = "pnlCommand";
            this.pnlCommand.Size = new System.Drawing.Size(912, 34);
            this.pnlCommand.TabIndex = 0;
            // 
            // pnlDisk
            // 
            this.pnlDisk.BackColor = System.Drawing.Color.Silver;
            this.pnlDisk.Controls.Add(this.rdoBDisk);
            this.pnlDisk.Controls.Add(this.rdoADisk);
            this.pnlDisk.Location = new System.Drawing.Point(453, 0);
            this.pnlDisk.Name = "pnlDisk";
            this.pnlDisk.Size = new System.Drawing.Size(60, 34);
            this.pnlDisk.TabIndex = 19;
            // 
            // rdoBDisk
            // 
            this.rdoBDisk.AutoSize = true;
            this.rdoBDisk.Location = new System.Drawing.Point(9, 17);
            this.rdoBDisk.Name = "rdoBDisk";
            this.rdoBDisk.Size = new System.Drawing.Size(43, 16);
            this.rdoBDisk.TabIndex = 9;
            this.rdoBDisk.Text = "B盘";
            this.rdoBDisk.UseVisualStyleBackColor = true;
            // 
            // rdoADisk
            // 
            this.rdoADisk.AutoSize = true;
            this.rdoADisk.Checked = true;
            this.rdoADisk.Location = new System.Drawing.Point(9, 1);
            this.rdoADisk.Name = "rdoADisk";
            this.rdoADisk.Size = new System.Drawing.Size(43, 16);
            this.rdoADisk.TabIndex = 8;
            this.rdoADisk.TabStop = true;
            this.rdoADisk.Text = "A盘";
            this.rdoADisk.UseVisualStyleBackColor = true;
            this.rdoADisk.CheckedChanged += new System.EventHandler(this.rdoADisk_CheckedChanged);
            // 
            // btnReLoad
            // 
            this.btnReLoad.Location = new System.Drawing.Point(535, 4);
            this.btnReLoad.Name = "btnReLoad";
            this.btnReLoad.Size = new System.Drawing.Size(60, 26);
            this.btnReLoad.TabIndex = 16;
            this.btnReLoad.Text = "刷新";
            this.btnReLoad.UseVisualStyleBackColor = true;
            this.btnReLoad.Click += new System.EventHandler(this.btnReLoad_Click);
            // 
            // btnPatch
            // 
            this.btnPatch.Location = new System.Drawing.Point(601, 4);
            this.btnPatch.Name = "btnPatch";
            this.btnPatch.Size = new System.Drawing.Size(60, 26);
            this.btnPatch.TabIndex = 15;
            this.btnPatch.Text = "打包";
            this.btnPatch.UseVisualStyleBackColor = true;
            this.btnPatch.Click += new System.EventHandler(this.btnPatch_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(734, 4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(60, 26);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "导入";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(668, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(60, 26);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(827, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 26);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlEditor
            // 
            this.pnlEditor.Controls.Add(this.txtJp);
            this.pnlEditor.Controls.Add(this.txtCn);
            this.pnlEditor.Controls.Add(this.fileList);
            this.pnlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEditor.Location = new System.Drawing.Point(0, 0);
            this.pnlEditor.Name = "pnlEditor";
            this.pnlEditor.Size = new System.Drawing.Size(912, 545);
            this.pnlEditor.TabIndex = 1;
            // 
            // txtJp
            // 
            this.txtJp.BackColor = System.Drawing.SystemColors.Window;
            this.txtJp.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtJp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJp.Location = new System.Drawing.Point(210, 270);
            this.txtJp.Name = "txtJp";
            this.txtJp.OtherRichTextBox = null;
            this.txtJp.ReadOnly = true;
            this.txtJp.Size = new System.Drawing.Size(702, 270);
            this.txtJp.TabIndex = 2;
            this.txtJp.Text = "";
            // 
            // txtCn
            // 
            this.txtCn.BackColor = System.Drawing.SystemColors.Window;
            this.txtCn.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtCn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCn.Location = new System.Drawing.Point(210, 0);
            this.txtCn.Name = "txtCn";
            this.txtCn.OtherRichTextBox = null;
            this.txtCn.Size = new System.Drawing.Size(702, 270);
            this.txtCn.TabIndex = 0;
            this.txtCn.Text = "";
            // 
            // fileList
            // 
            this.fileList.Dock = System.Windows.Forms.DockStyle.Left;
            this.fileList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileList.FormattingEnabled = true;
            this.fileList.ItemHeight = 20;
            this.fileList.Location = new System.Drawing.Point(0, 0);
            this.fileList.Name = "fileList";
            this.fileList.Size = new System.Drawing.Size(210, 544);
            this.fileList.TabIndex = 1;
            this.fileList.SelectedIndexChanged += new System.EventHandler(this.fileList_SelectedIndexChanged);
            // 
            // BaseTextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 604);
            this.Controls.Add(this.pnlEditor);
            this.Controls.Add(this.pnlCommand);
            this.Name = "BaseTextEditor";
            this.Text = "文本编辑";
            this.Controls.SetChildIndex(this.pnlTopBody, 0);
            this.Controls.SetChildIndex(this.pnlCommand, 0);
            this.Controls.SetChildIndex(this.pnlEditor, 0);
            this.pnlCommand.ResumeLayout(false);
            this.pnlDisk.ResumeLayout(false);
            this.pnlDisk.PerformLayout();
            this.pnlEditor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel pnlCommand;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlEditor;
        private RichTextBoxEx txtCn;
        private System.Windows.Forms.ListBox fileList;
        private RichTextBoxEx txtJp;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnReLoad;
        private System.Windows.Forms.Button btnPatch;
        private System.Windows.Forms.Panel pnlDisk;
        private System.Windows.Forms.RadioButton rdoBDisk;
        private System.Windows.Forms.RadioButton rdoADisk;
    }
}
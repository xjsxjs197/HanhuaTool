using Hanhua.Common;
namespace Hanhua.TextEditTools.Bio1Edit
{
    partial class Bio1TextEditor
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
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPatch = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.btnCreateFont = new System.Windows.Forms.Button();
            this.btnCnFontSave = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlEditor = new System.Windows.Forms.Panel();
            this.txtCnEdit = new Hanhua.Common.RichTextBoxEx();
            this.txtJpEdit = new Hanhua.Common.RichTextBoxEx();
            this.lstFile = new System.Windows.Forms.ListBox();
            this.pnlCommand.SuspendLayout();
            this.pnlEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCommand
            // 
            this.pnlCommand.BackColor = System.Drawing.Color.SandyBrown;
            this.pnlCommand.Controls.Add(this.btnImport);
            this.pnlCommand.Controls.Add(this.btnExport);
            this.pnlCommand.Controls.Add(this.btnPatch);
            this.pnlCommand.Controls.Add(this.btnCheck);
            this.pnlCommand.Controls.Add(this.btnCreateFont);
            this.pnlCommand.Controls.Add(this.btnCnFontSave);
            this.pnlCommand.Controls.Add(this.btnSave);
            this.pnlCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCommand.Location = new System.Drawing.Point(0, 390);
            this.pnlCommand.Name = "pnlCommand";
            this.pnlCommand.Size = new System.Drawing.Size(844, 37);
            this.pnlCommand.TabIndex = 0;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(119, 6);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 26);
            this.btnImport.TabIndex = 34;
            this.btnImport.Text = "导入文本";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(23, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(90, 26);
            this.btnExport.TabIndex = 33;
            this.btnExport.Text = "导出文本";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPatch
            // 
            this.btnPatch.Location = new System.Drawing.Point(215, 6);
            this.btnPatch.Name = "btnPatch";
            this.btnPatch.Size = new System.Drawing.Size(90, 26);
            this.btnPatch.TabIndex = 31;
            this.btnPatch.Text = "开始打包";
            this.btnPatch.UseVisualStyleBackColor = true;
            this.btnPatch.Click += new System.EventHandler(this.btnPatch_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(529, 6);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(90, 26);
            this.btnCheck.TabIndex = 27;
            this.btnCheck.Text = "检查翻译";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // btnCreateFont
            // 
            this.btnCreateFont.Location = new System.Drawing.Point(433, 6);
            this.btnCreateFont.Name = "btnCreateFont";
            this.btnCreateFont.Size = new System.Drawing.Size(90, 26);
            this.btnCreateFont.TabIndex = 26;
            this.btnCreateFont.Text = "字库做成";
            this.btnCreateFont.UseVisualStyleBackColor = true;
            this.btnCreateFont.Click += new System.EventHandler(this.btnCreateFont_Click);
            // 
            // btnCnFontSave
            // 
            this.btnCnFontSave.Enabled = false;
            this.btnCnFontSave.Location = new System.Drawing.Point(721, 6);
            this.btnCnFontSave.Name = "btnCnFontSave";
            this.btnCnFontSave.Size = new System.Drawing.Size(117, 26);
            this.btnCnFontSave.TabIndex = 25;
            this.btnCnFontSave.Text = "Reset字库文字";
            this.btnCnFontSave.UseVisualStyleBackColor = true;
            this.btnCnFontSave.Click += new System.EventHandler(this.btnCnFontSave_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(625, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 26);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "保存翻译";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlEditor
            // 
            this.pnlEditor.Controls.Add(this.txtCnEdit);
            this.pnlEditor.Controls.Add(this.txtJpEdit);
            this.pnlEditor.Controls.Add(this.lstFile);
            this.pnlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEditor.Location = new System.Drawing.Point(0, 0);
            this.pnlEditor.Name = "pnlEditor";
            this.pnlEditor.Size = new System.Drawing.Size(844, 390);
            this.pnlEditor.TabIndex = 1;
            // 
            // txtCnEdit
            // 
            this.txtCnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCnEdit.Location = new System.Drawing.Point(499, 0);
            this.txtCnEdit.Name = "txtCnEdit";
            this.txtCnEdit.OtherRichTextBox = null;
            this.txtCnEdit.Size = new System.Drawing.Size(345, 390);
            this.txtCnEdit.TabIndex = 1;
            this.txtCnEdit.Text = "";
            // 
            // txtJpEdit
            // 
            this.txtJpEdit.BackColor = System.Drawing.SystemColors.Window;
            this.txtJpEdit.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtJpEdit.Location = new System.Drawing.Point(185, 0);
            this.txtJpEdit.Name = "txtJpEdit";
            this.txtJpEdit.OtherRichTextBox = null;
            this.txtJpEdit.ReadOnly = true;
            this.txtJpEdit.Size = new System.Drawing.Size(314, 390);
            this.txtJpEdit.TabIndex = 0;
            this.txtJpEdit.Text = "";
            // 
            // lstFile
            // 
            this.lstFile.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstFile.FormattingEnabled = true;
            this.lstFile.ItemHeight = 12;
            this.lstFile.Location = new System.Drawing.Point(0, 0);
            this.lstFile.Name = "lstFile";
            this.lstFile.Size = new System.Drawing.Size(185, 388);
            this.lstFile.TabIndex = 2;
            this.lstFile.SelectedIndexChanged += new System.EventHandler(this.lstFile_SelectedIndexChanged);
            // 
            // Bio1TextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 452);
            this.Controls.Add(this.pnlEditor);
            this.Controls.Add(this.pnlCommand);
            this.Name = "Bio1TextEditor";
            this.Text = "Bio1文本编辑";
            this.Controls.SetChildIndex(this.pnlCommand, 0);
            this.Controls.SetChildIndex(this.pnlEditor, 0);
            this.pnlCommand.ResumeLayout(false);
            this.pnlEditor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCommand;
        private System.Windows.Forms.Panel pnlEditor;
        private RichTextBoxEx txtJpEdit;
        private RichTextBoxEx txtCnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCnFontSave;
        private System.Windows.Forms.Button btnCreateFont;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.ListBox lstFile;
        private System.Windows.Forms.Button btnPatch;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
    }
}
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
            this.btnCreateFont = new System.Windows.Forms.Button();
            this.btnCnFontSave = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlEditor = new System.Windows.Forms.Panel();
            this.txtCnEdit = new Hanhua.Common.RichTextBoxEx();
            this.txtJpEdit = new Hanhua.Common.RichTextBoxEx();
            this.lstFile = new System.Windows.Forms.ListBox();
            this.pnlDisk = new System.Windows.Forms.Panel();
            this.rdoBDisk = new System.Windows.Forms.RadioButton();
            this.rdoADisk = new System.Windows.Forms.RadioButton();
            this.rdoWii = new System.Windows.Forms.RadioButton();
            this.rdoNgc = new System.Windows.Forms.RadioButton();
            this.pnlCommand.SuspendLayout();
            this.pnlEditor.SuspendLayout();
            this.pnlDisk.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Size = new System.Drawing.Size(1125, 534);
            // 
            // pnlCommand
            // 
            this.pnlCommand.BackColor = System.Drawing.Color.SandyBrown;
            this.pnlCommand.Controls.Add(this.rdoNgc);
            this.pnlCommand.Controls.Add(this.rdoWii);
            this.pnlCommand.Controls.Add(this.pnlDisk);
            this.pnlCommand.Controls.Add(this.btnImport);
            this.pnlCommand.Controls.Add(this.btnExport);
            this.pnlCommand.Controls.Add(this.btnCreateFont);
            this.pnlCommand.Controls.Add(this.btnCnFontSave);
            this.pnlCommand.Controls.Add(this.btnSave);
            this.pnlCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCommand.Location = new System.Drawing.Point(0, 488);
            this.pnlCommand.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCommand.Name = "pnlCommand";
            this.pnlCommand.Size = new System.Drawing.Size(1125, 46);
            this.pnlCommand.TabIndex = 0;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(159, 8);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(120, 32);
            this.btnImport.TabIndex = 34;
            this.btnImport.Text = "导入文本";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(31, 8);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 32);
            this.btnExport.TabIndex = 33;
            this.btnExport.Text = "导出文本";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnCreateFont
            // 
            this.btnCreateFont.Location = new System.Drawing.Point(705, 8);
            this.btnCreateFont.Margin = new System.Windows.Forms.Padding(4);
            this.btnCreateFont.Name = "btnCreateFont";
            this.btnCreateFont.Size = new System.Drawing.Size(120, 32);
            this.btnCreateFont.TabIndex = 26;
            this.btnCreateFont.Text = "字库做成";
            this.btnCreateFont.UseVisualStyleBackColor = true;
            this.btnCreateFont.Click += new System.EventHandler(this.btnCreateFont_Click);
            // 
            // btnCnFontSave
            // 
            this.btnCnFontSave.Location = new System.Drawing.Point(961, 8);
            this.btnCnFontSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnCnFontSave.Name = "btnCnFontSave";
            this.btnCnFontSave.Size = new System.Drawing.Size(156, 32);
            this.btnCnFontSave.TabIndex = 25;
            this.btnCnFontSave.Text = "刷新";
            this.btnCnFontSave.UseVisualStyleBackColor = true;
            this.btnCnFontSave.Click += new System.EventHandler(this.btnCnFontSave_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(833, 8);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 32);
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
            this.pnlEditor.Margin = new System.Windows.Forms.Padding(4);
            this.pnlEditor.Name = "pnlEditor";
            this.pnlEditor.Size = new System.Drawing.Size(1125, 488);
            this.pnlEditor.TabIndex = 1;
            // 
            // txtCnEdit
            // 
            this.txtCnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCnEdit.Location = new System.Drawing.Point(662, 0);
            this.txtCnEdit.Margin = new System.Windows.Forms.Padding(4);
            this.txtCnEdit.Name = "txtCnEdit";
            this.txtCnEdit.OtherRichTextBox = null;
            this.txtCnEdit.Size = new System.Drawing.Size(463, 488);
            this.txtCnEdit.TabIndex = 1;
            this.txtCnEdit.Text = "";
            // 
            // txtJpEdit
            // 
            this.txtJpEdit.BackColor = System.Drawing.SystemColors.Window;
            this.txtJpEdit.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtJpEdit.Location = new System.Drawing.Point(245, 0);
            this.txtJpEdit.Margin = new System.Windows.Forms.Padding(4);
            this.txtJpEdit.Name = "txtJpEdit";
            this.txtJpEdit.OtherRichTextBox = null;
            this.txtJpEdit.ReadOnly = true;
            this.txtJpEdit.Size = new System.Drawing.Size(417, 488);
            this.txtJpEdit.TabIndex = 0;
            this.txtJpEdit.Text = "";
            // 
            // lstFile
            // 
            this.lstFile.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstFile.FormattingEnabled = true;
            this.lstFile.ItemHeight = 15;
            this.lstFile.Location = new System.Drawing.Point(0, 0);
            this.lstFile.Margin = new System.Windows.Forms.Padding(4);
            this.lstFile.Name = "lstFile";
            this.lstFile.Size = new System.Drawing.Size(245, 484);
            this.lstFile.TabIndex = 2;
            this.lstFile.SelectedIndexChanged += new System.EventHandler(this.lstFile_SelectedIndexChanged);
            // 
            // pnlDisk
            // 
            this.pnlDisk.BackColor = System.Drawing.Color.Silver;
            this.pnlDisk.Controls.Add(this.rdoBDisk);
            this.pnlDisk.Controls.Add(this.rdoADisk);
            this.pnlDisk.Location = new System.Drawing.Point(579, 2);
            this.pnlDisk.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDisk.Name = "pnlDisk";
            this.pnlDisk.Size = new System.Drawing.Size(80, 42);
            this.pnlDisk.TabIndex = 35;
            // 
            // rdoBDisk
            // 
            this.rdoBDisk.AutoSize = true;
            this.rdoBDisk.Enabled = false;
            this.rdoBDisk.Location = new System.Drawing.Point(12, 21);
            this.rdoBDisk.Margin = new System.Windows.Forms.Padding(4);
            this.rdoBDisk.Name = "rdoBDisk";
            this.rdoBDisk.Size = new System.Drawing.Size(51, 19);
            this.rdoBDisk.TabIndex = 9;
            this.rdoBDisk.Text = "B盘";
            this.rdoBDisk.UseVisualStyleBackColor = true;
            // 
            // rdoADisk
            // 
            this.rdoADisk.AutoSize = true;
            this.rdoADisk.Enabled = false;
            this.rdoADisk.Location = new System.Drawing.Point(12, 1);
            this.rdoADisk.Margin = new System.Windows.Forms.Padding(4);
            this.rdoADisk.Name = "rdoADisk";
            this.rdoADisk.Size = new System.Drawing.Size(51, 19);
            this.rdoADisk.TabIndex = 8;
            this.rdoADisk.Text = "A盘";
            this.rdoADisk.UseVisualStyleBackColor = true;
            // 
            // rdoWii
            // 
            this.rdoWii.AutoSize = true;
            this.rdoWii.Checked = true;
            this.rdoWii.Location = new System.Drawing.Point(349, 15);
            this.rdoWii.Margin = new System.Windows.Forms.Padding(4);
            this.rdoWii.Name = "rdoWii";
            this.rdoWii.Size = new System.Drawing.Size(52, 19);
            this.rdoWii.TabIndex = 36;
            this.rdoWii.Text = "Wii";
            this.rdoWii.UseVisualStyleBackColor = true;
            this.rdoWii.CheckedChanged += new System.EventHandler(this.rdoWii_CheckedChanged);
            // 
            // rdoNgc
            // 
            this.rdoNgc.AutoSize = true;
            this.rdoNgc.Location = new System.Drawing.Point(421, 15);
            this.rdoNgc.Margin = new System.Windows.Forms.Padding(4);
            this.rdoNgc.Name = "rdoNgc";
            this.rdoNgc.Size = new System.Drawing.Size(52, 19);
            this.rdoNgc.TabIndex = 37;
            this.rdoNgc.Text = "Ngc";
            this.rdoNgc.UseVisualStyleBackColor = true;
            // 
            // Bio1TextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 565);
            this.Controls.Add(this.pnlEditor);
            this.Controls.Add(this.pnlCommand);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Bio1TextEditor";
            this.Text = "Bio1文本编辑";
            this.Controls.SetChildIndex(this.pnlTopBody, 0);
            this.Controls.SetChildIndex(this.pnlCommand, 0);
            this.Controls.SetChildIndex(this.pnlEditor, 0);
            this.pnlCommand.ResumeLayout(false);
            this.pnlCommand.PerformLayout();
            this.pnlEditor.ResumeLayout(false);
            this.pnlDisk.ResumeLayout(false);
            this.pnlDisk.PerformLayout();
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
        private System.Windows.Forms.ListBox lstFile;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel pnlDisk;
        private System.Windows.Forms.RadioButton rdoBDisk;
        private System.Windows.Forms.RadioButton rdoADisk;
        private System.Windows.Forms.RadioButton rdoNgc;
        private System.Windows.Forms.RadioButton rdoWii;
    }
}
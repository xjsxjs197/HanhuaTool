namespace Hanhua.Common
{
    partial class SingleFileResult
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
            this.lblFile = new System.Windows.Forms.Label();
            this.lblDecoder = new System.Windows.Forms.Label();
            this.ddlDecoder = new System.Windows.Forms.ComboBox();
            this.lblRange = new System.Windows.Forms.Label();
            this.txtStartPos = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEndPos = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.RichTextBox();
            this.chkRange = new System.Windows.Forms.CheckBox();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cmbUnicodeType = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtReSearch = new System.Windows.Forms.TextBox();
            this.btnReSearch = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(9, 11);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(47, 12);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "文件名：";
            // 
            // lblDecoder
            // 
            this.lblDecoder.AutoSize = true;
            this.lblDecoder.Location = new System.Drawing.Point(9, 36);
            this.lblDecoder.Name = "lblDecoder";
            this.lblDecoder.Size = new System.Drawing.Size(47, 12);
            this.lblDecoder.TabIndex = 2;
            this.lblDecoder.Text = "解码器：";
            // 
            // ddlDecoder
            // 
            this.ddlDecoder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDecoder.FormattingEnabled = true;
            this.ddlDecoder.Items.AddRange(new object[] {
            "Shift-Jis",
            "Unicode",
            "Utf-8",
            "JIS(20932)",
            "iso-2022-jp(50220)",
            "euc-jp(51932)",
            "Mac_jp(10001)",
            "Jis1 Allow 1 byte Kana(50221)",
            "Jis1 Allow 1 byte Kana - SO/SI(50222)",
            "Utf32",
            "Utf7"});
            this.ddlDecoder.Location = new System.Drawing.Point(82, 33);
            this.ddlDecoder.Name = "ddlDecoder";
            this.ddlDecoder.Size = new System.Drawing.Size(212, 20);
            this.ddlDecoder.TabIndex = 3;
            this.ddlDecoder.SelectedIndexChanged += new System.EventHandler(this.ddlDecoder_SelectedIndexChanged);
            // 
            // lblRange
            // 
            this.lblRange.AutoSize = true;
            this.lblRange.Location = new System.Drawing.Point(9, 65);
            this.lblRange.Name = "lblRange";
            this.lblRange.Size = new System.Drawing.Size(43, 12);
            this.lblRange.TabIndex = 4;
            this.lblRange.Text = "范  围：";
            // 
            // txtStartPos
            // 
            this.txtStartPos.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txtStartPos.Location = new System.Drawing.Point(83, 62);
            this.txtStartPos.Name = "txtStartPos";
            this.txtStartPos.Size = new System.Drawing.Size(74, 19);
            this.txtStartPos.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(183, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "--";
            // 
            // txtEndPos
            // 
            this.txtEndPos.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txtEndPos.Location = new System.Drawing.Point(220, 62);
            this.txtEndPos.Name = "txtEndPos";
            this.txtEndPos.Size = new System.Drawing.Size(74, 19);
            this.txtEndPos.TabIndex = 7;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(408, 33);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(85, 27);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "再分析";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(0, 6);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(671, 256);
            this.txtResult.TabIndex = 9;
            this.txtResult.Text = "";
            // 
            // chkRange
            // 
            this.chkRange.AutoSize = true;
            this.chkRange.Checked = true;
            this.chkRange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRange.Location = new System.Drawing.Point(313, 65);
            this.chkRange.Name = "chkRange";
            this.chkRange.Size = new System.Drawing.Size(72, 16);
            this.chkRange.TabIndex = 10;
            this.chkRange.Text = "范围分析";
            this.chkRange.UseVisualStyleBackColor = true;
            // 
            // txtFileName
            // 
            this.txtFileName.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txtFileName.Location = new System.Drawing.Point(82, 6);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(303, 19);
            this.txtFileName.TabIndex = 11;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(408, 4);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(85, 27);
            this.btnOpenFile.TabIndex = 12;
            this.btnOpenFile.Text = "换文件";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "所有文件|*.*";
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // cmbUnicodeType
            // 
            this.cmbUnicodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnicodeType.FormattingEnabled = true;
            this.cmbUnicodeType.Items.AddRange(new object[] {
            "Big Endian",
            "Little Endian"});
            this.cmbUnicodeType.Location = new System.Drawing.Point(313, 33);
            this.cmbUnicodeType.Name = "cmbUnicodeType";
            this.cmbUnicodeType.Size = new System.Drawing.Size(72, 20);
            this.cmbUnicodeType.TabIndex = 14;
            this.cmbUnicodeType.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtReSearch);
            this.panel1.Controls.Add(this.btnReSearch);
            this.panel1.Controls.Add(this.btnOpenFile);
            this.panel1.Controls.Add(this.cmbUnicodeType);
            this.panel1.Controls.Add(this.txtFileName);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.chkRange);
            this.panel1.Controls.Add(this.txtEndPos);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtStartPos);
            this.panel1.Controls.Add(this.lblRange);
            this.panel1.Controls.Add(this.ddlDecoder);
            this.panel1.Controls.Add(this.lblDecoder);
            this.panel1.Controls.Add(this.lblFile);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(671, 103);
            this.panel1.TabIndex = 15;
            // 
            // txtReSearch
            // 
            this.txtReSearch.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txtReSearch.Location = new System.Drawing.Point(499, 65);
            this.txtReSearch.Name = "txtReSearch";
            this.txtReSearch.Size = new System.Drawing.Size(161, 19);
            this.txtReSearch.TabIndex = 16;
            // 
            // btnReSearch
            // 
            this.btnReSearch.Location = new System.Drawing.Point(408, 62);
            this.btnReSearch.Name = "btnReSearch";
            this.btnReSearch.Size = new System.Drawing.Size(85, 27);
            this.btnReSearch.TabIndex = 15;
            this.btnReSearch.Text = "结果中再查询";
            this.btnReSearch.UseVisualStyleBackColor = true;
            this.btnReSearch.Click += new System.EventHandler(this.btnReSearch_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtResult);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 103);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.panel2.Size = new System.Drawing.Size(671, 262);
            this.panel2.TabIndex = 16;
            // 
            // SingleFileResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 365);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SingleFileResult";
            this.Text = "文本结果查看";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label lblDecoder;
        private System.Windows.Forms.ComboBox ddlDecoder;
        private System.Windows.Forms.Label lblRange;
        private System.Windows.Forms.TextBox txtStartPos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEndPos;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.RichTextBox txtResult;
        private System.Windows.Forms.CheckBox chkRange;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ComboBox cmbUnicodeType;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnReSearch;
        private System.Windows.Forms.TextBox txtReSearch;
    }
}
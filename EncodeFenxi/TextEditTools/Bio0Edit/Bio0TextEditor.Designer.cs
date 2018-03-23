using Hanhua.Common;
namespace Hanhua.TextEditTools.Bio0Edit
{
    partial class Bio0TextEditor
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
            this.rdoImg = new System.Windows.Forms.RadioButton();
            this.rdoAll = new System.Windows.Forms.RadioButton();
            this.btnPatch = new System.Windows.Forms.Button();
            this.rdoFont4 = new System.Windows.Forms.RadioButton();
            this.rdoFont3 = new System.Windows.Forms.RadioButton();
            this.rdoFont2 = new System.Windows.Forms.RadioButton();
            this.rdoFont1 = new System.Windows.Forms.RadioButton();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.txtSearchKey = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCreateCnFont = new System.Windows.Forms.Button();
            this.pnlEditor = new System.Windows.Forms.Panel();
            this.txtJp = new Hanhua.Common.RichTextBoxEx();
            this.txtCn = new Hanhua.Common.RichTextBoxEx();
            this.fileList = new System.Windows.Forms.ListBox();
            this.pnlCommand.SuspendLayout();
            this.pnlEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCommand
            // 
            this.pnlCommand.BackColor = System.Drawing.Color.SandyBrown;
            this.pnlCommand.Controls.Add(this.rdoImg);
            this.pnlCommand.Controls.Add(this.rdoAll);
            this.pnlCommand.Controls.Add(this.btnPatch);
            this.pnlCommand.Controls.Add(this.rdoFont4);
            this.pnlCommand.Controls.Add(this.rdoFont3);
            this.pnlCommand.Controls.Add(this.rdoFont2);
            this.pnlCommand.Controls.Add(this.rdoFont1);
            this.pnlCommand.Controls.Add(this.btnImport);
            this.pnlCommand.Controls.Add(this.btnExport);
            this.pnlCommand.Controls.Add(this.txtSearchKey);
            this.pnlCommand.Controls.Add(this.btnSearch);
            this.pnlCommand.Controls.Add(this.btnSave);
            this.pnlCommand.Controls.Add(this.btnCreateCnFont);
            this.pnlCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCommand.Location = new System.Drawing.Point(0, 547);
            this.pnlCommand.Name = "pnlCommand";
            this.pnlCommand.Size = new System.Drawing.Size(912, 35);
            this.pnlCommand.TabIndex = 0;
            // 
            // rdoImg
            // 
            this.rdoImg.AutoSize = true;
            this.rdoImg.Location = new System.Drawing.Point(268, 10);
            this.rdoImg.Name = "rdoImg";
            this.rdoImg.Size = new System.Drawing.Size(47, 16);
            this.rdoImg.TabIndex = 18;
            this.rdoImg.TabStop = true;
            this.rdoImg.Text = "图片";
            this.rdoImg.UseVisualStyleBackColor = true;
            // 
            // rdoAll
            // 
            this.rdoAll.AutoSize = true;
            this.rdoAll.Location = new System.Drawing.Point(324, 10);
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(47, 16);
            this.rdoAll.TabIndex = 17;
            this.rdoAll.TabStop = true;
            this.rdoAll.Text = "所有";
            this.rdoAll.UseVisualStyleBackColor = true;
            // 
            // btnPatch
            // 
            this.btnPatch.Location = new System.Drawing.Point(519, 5);
            this.btnPatch.Name = "btnPatch";
            this.btnPatch.Size = new System.Drawing.Size(60, 26);
            this.btnPatch.TabIndex = 16;
            this.btnPatch.Text = "打包";
            this.btnPatch.UseVisualStyleBackColor = true;
            this.btnPatch.Click += new System.EventHandler(this.btnPatch_Click);
            // 
            // rdoFont4
            // 
            this.rdoFont4.AutoSize = true;
            this.rdoFont4.Location = new System.Drawing.Point(207, 10);
            this.rdoFont4.Name = "rdoFont4";
            this.rdoFont4.Size = new System.Drawing.Size(55, 16);
            this.rdoFont4.TabIndex = 15;
            this.rdoFont4.TabStop = true;
            this.rdoFont4.Text = "font04";
            this.rdoFont4.UseVisualStyleBackColor = true;
            // 
            // rdoFont3
            // 
            this.rdoFont3.AutoSize = true;
            this.rdoFont3.Location = new System.Drawing.Point(142, 10);
            this.rdoFont3.Name = "rdoFont3";
            this.rdoFont3.Size = new System.Drawing.Size(55, 16);
            this.rdoFont3.TabIndex = 14;
            this.rdoFont3.TabStop = true;
            this.rdoFont3.Text = "font03";
            this.rdoFont3.UseVisualStyleBackColor = true;
            // 
            // rdoFont2
            // 
            this.rdoFont2.AutoSize = true;
            this.rdoFont2.Location = new System.Drawing.Point(77, 10);
            this.rdoFont2.Name = "rdoFont2";
            this.rdoFont2.Size = new System.Drawing.Size(55, 16);
            this.rdoFont2.TabIndex = 13;
            this.rdoFont2.TabStop = true;
            this.rdoFont2.Text = "font02";
            this.rdoFont2.UseVisualStyleBackColor = true;
            // 
            // rdoFont1
            // 
            this.rdoFont1.AutoSize = true;
            this.rdoFont1.Checked = true;
            this.rdoFont1.Location = new System.Drawing.Point(12, 10);
            this.rdoFont1.Name = "rdoFont1";
            this.rdoFont1.Size = new System.Drawing.Size(55, 16);
            this.rdoFont1.TabIndex = 12;
            this.rdoFont1.TabStop = true;
            this.rdoFont1.Text = "font01";
            this.rdoFont1.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(453, 5);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(60, 26);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "导入";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(387, 5);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(60, 26);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // txtSearchKey
            // 
            this.txtSearchKey.Location = new System.Drawing.Point(622, 9);
            this.txtSearchKey.Name = "txtSearchKey";
            this.txtSearchKey.Size = new System.Drawing.Size(47, 19);
            this.txtSearchKey.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(675, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(53, 26);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "查找";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSaveAddr_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(842, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(64, 26);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCreateCnFont
            // 
            this.btnCreateCnFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateCnFont.Location = new System.Drawing.Point(734, 5);
            this.btnCreateCnFont.Name = "btnCreateCnFont";
            this.btnCreateCnFont.Size = new System.Drawing.Size(106, 26);
            this.btnCreateCnFont.TabIndex = 0;
            this.btnCreateCnFont.Text = "生成中文字库";
            this.btnCreateCnFont.UseVisualStyleBackColor = true;
            this.btnCreateCnFont.Click += new System.EventHandler(this.btnCreateCnFont_Click);
            // 
            // pnlEditor
            // 
            this.pnlEditor.Controls.Add(this.txtJp);
            this.pnlEditor.Controls.Add(this.txtCn);
            this.pnlEditor.Controls.Add(this.fileList);
            this.pnlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEditor.Location = new System.Drawing.Point(0, 0);
            this.pnlEditor.Name = "pnlEditor";
            this.pnlEditor.Size = new System.Drawing.Size(912, 547);
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
            // Bio0TextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 607);
            this.Controls.Add(this.pnlEditor);
            this.Controls.Add(this.pnlCommand);
            this.Name = "Bio0TextEditor";
            this.Text = "Bio0文本查看";
            this.Controls.SetChildIndex(this.pnlCommand, 0);
            this.Controls.SetChildIndex(this.pnlEditor, 0);
            this.pnlCommand.ResumeLayout(false);
            this.pnlCommand.PerformLayout();
            this.pnlEditor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCommand;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCreateCnFont;
        private System.Windows.Forms.Panel pnlEditor;
        private RichTextBoxEx txtCn;
        private System.Windows.Forms.ListBox fileList;
        private RichTextBoxEx txtJp;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearchKey;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.RadioButton rdoFont4;
        private System.Windows.Forms.RadioButton rdoFont3;
        private System.Windows.Forms.RadioButton rdoFont2;
        private System.Windows.Forms.RadioButton rdoFont1;
        private System.Windows.Forms.Button btnPatch;
        private System.Windows.Forms.RadioButton rdoAll;
        private System.Windows.Forms.RadioButton rdoImg;
    }
}
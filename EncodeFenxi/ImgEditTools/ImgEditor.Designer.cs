namespace Hanhua.ImgEditTools
{
    partial class ImgEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDoWork = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblPalette = new System.Windows.Forms.Label();
            this.cmbTimPalette = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbImgType = new System.Windows.Forms.ComboBox();
            this.btnSelFile = new System.Windows.Forms.Button();
            this.chkCheckOther = new System.Windows.Forms.CheckBox();
            this.btnReadDic = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.imgGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.lstImg = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Margin = new System.Windows.Forms.Padding(4);
            this.pnlTopBody.Size = new System.Drawing.Size(887, 291);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDoWork);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.lblPalette);
            this.panel1.Controls.Add(this.cmbTimPalette);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbImgType);
            this.panel1.Controls.Add(this.btnSelFile);
            this.panel1.Controls.Add(this.chkCheckOther);
            this.panel1.Controls.Add(this.btnReadDic);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 259);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(887, 32);
            this.panel1.TabIndex = 0;
            // 
            // btnDoWork
            // 
            this.btnDoWork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDoWork.Location = new System.Drawing.Point(585, 5);
            this.btnDoWork.Name = "btnDoWork";
            this.btnDoWork.Size = new System.Drawing.Size(68, 23);
            this.btnDoWork.TabIndex = 11;
            this.btnDoWork.Text = "各种操作";
            this.btnDoWork.UseVisualStyleBackColor = true;
            this.btnDoWork.Click += new System.EventHandler(this.btnDoWork_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(659, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "保存修改";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblPalette
            // 
            this.lblPalette.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPalette.AutoSize = true;
            this.lblPalette.Location = new System.Drawing.Point(479, 10);
            this.lblPalette.Name = "lblPalette";
            this.lblPalette.Size = new System.Drawing.Size(41, 12);
            this.lblPalette.TabIndex = 9;
            this.lblPalette.Text = "调色板";
            // 
            // cmbTimPalette
            // 
            this.cmbTimPalette.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTimPalette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimPalette.FormattingEnabled = true;
            this.cmbTimPalette.Location = new System.Drawing.Point(526, 7);
            this.cmbTimPalette.Name = "cmbTimPalette";
            this.cmbTimPalette.Size = new System.Drawing.Size(45, 20);
            this.cmbTimPalette.TabIndex = 8;
            this.cmbTimPalette.SelectedIndexChanged += new System.EventHandler(this.cmbTimPalette_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "类型";
            // 
            // cmbImgType
            // 
            this.cmbImgType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImgType.FormattingEnabled = true;
            this.cmbImgType.Location = new System.Drawing.Point(45, 6);
            this.cmbImgType.Name = "cmbImgType";
            this.cmbImgType.Size = new System.Drawing.Size(113, 20);
            this.cmbImgType.TabIndex = 6;
            this.cmbImgType.SelectedIndexChanged += new System.EventHandler(this.cmbImgType_SelectedIndexChanged);
            // 
            // btnSelFile
            // 
            this.btnSelFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelFile.Location = new System.Drawing.Point(807, 5);
            this.btnSelFile.Name = "btnSelFile";
            this.btnSelFile.Size = new System.Drawing.Size(68, 23);
            this.btnSelFile.TabIndex = 5;
            this.btnSelFile.Text = "选择文件";
            this.btnSelFile.UseVisualStyleBackColor = true;
            this.btnSelFile.Click += new System.EventHandler(this.btnSelFile_Click);
            // 
            // chkCheckOther
            // 
            this.chkCheckOther.AutoSize = true;
            this.chkCheckOther.Checked = true;
            this.chkCheckOther.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCheckOther.Location = new System.Drawing.Point(186, 9);
            this.chkCheckOther.Name = "chkCheckOther";
            this.chkCheckOther.Size = new System.Drawing.Size(138, 16);
            this.chkCheckOther.TabIndex = 4;
            this.chkCheckOther.Text = "分析非{0}后缀的文件";
            this.chkCheckOther.UseVisualStyleBackColor = true;
            // 
            // btnReadDic
            // 
            this.btnReadDic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReadDic.Location = new System.Drawing.Point(733, 5);
            this.btnReadDic.Name = "btnReadDic";
            this.btnReadDic.Size = new System.Drawing.Size(68, 23);
            this.btnReadDic.TabIndex = 3;
            this.btnReadDic.Text = "读取目录";
            this.btnReadDic.UseVisualStyleBackColor = true;
            this.btnReadDic.Click += new System.EventHandler(this.btnReadDic_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.imgGrid);
            this.panel2.Controls.Add(this.lstImg);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(887, 259);
            this.panel2.TabIndex = 1;
            // 
            // imgGrid
            // 
            this.imgGrid.AllowDrop = true;
            this.imgGrid.AllowUserToAddRows = false;
            this.imgGrid.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.imgGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.imgGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.imgGrid.ColumnHeadersVisible = false;
            this.imgGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.imgGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.imgGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgGrid.Location = new System.Drawing.Point(254, 0);
            this.imgGrid.Name = "imgGrid";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.imgGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.imgGrid.RowHeadersVisible = false;
            this.imgGrid.RowTemplate.Height = 11;
            this.imgGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.imgGrid.ShowCellErrors = false;
            this.imgGrid.ShowRowErrors = false;
            this.imgGrid.Size = new System.Drawing.Size(633, 259);
            this.imgGrid.TabIndex = 5;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.Width = 256;
            // 
            // lstImg
            // 
            this.lstImg.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstImg.FormattingEnabled = true;
            this.lstImg.ItemHeight = 12;
            this.lstImg.Location = new System.Drawing.Point(0, 0);
            this.lstImg.Name = "lstImg";
            this.lstImg.Size = new System.Drawing.Size(254, 256);
            this.lstImg.TabIndex = 0;
            this.lstImg.SelectedIndexChanged += new System.EventHandler(this.lstImg_SelectedIndexChanged);
            // 
            // ImgEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 316);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ImgEditor";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "{0}图片汉化专用工具";
            this.Controls.SetChildIndex(this.pnlTopBody, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox lstImg;
        private System.Windows.Forms.Button btnSelFile;
        private System.Windows.Forms.CheckBox chkCheckOther;
        private System.Windows.Forms.Button btnReadDic;
        private System.Windows.Forms.ComboBox cmbImgType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPalette;
        private System.Windows.Forms.ComboBox cmbTimPalette;
        private System.Windows.Forms.DataGridView imgGrid;
        private System.Windows.Forms.DataGridViewImageColumn Column1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDoWork;
    }
}
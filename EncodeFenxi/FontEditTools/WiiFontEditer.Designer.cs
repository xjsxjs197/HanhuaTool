namespace Hanhua.FontEditTools
{
    partial class WiiFontEditer
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnViewNewFont = new System.Windows.Forms.Button();
            this.btnCopyOldFont = new System.Windows.Forms.Button();
            this.btnFontSelect = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCreateCharMap = new System.Windows.Forms.Button();
            this.btnViewFontInfo = new System.Windows.Forms.Button();
            this.btnViewOldFont = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFontTest = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.fontGrid = new System.Windows.Forms.DataGridView();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Size = new System.Drawing.Size(847, 375);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtFontTest);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(847, 375);
            this.panel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.btnViewNewFont);
            this.groupBox2.Controls.Add(this.btnCopyOldFont);
            this.groupBox2.Controls.Add(this.btnFontSelect);
            this.groupBox2.Location = new System.Drawing.Point(734, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(106, 296);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "新字库操作";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(8, 175);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 25);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnViewNewFont
            // 
            this.btnViewNewFont.Location = new System.Drawing.Point(8, 224);
            this.btnViewNewFont.Name = "btnViewNewFont";
            this.btnViewNewFont.Size = new System.Drawing.Size(90, 25);
            this.btnViewNewFont.TabIndex = 8;
            this.btnViewNewFont.Text = "使用新字体";
            this.btnViewNewFont.UseVisualStyleBackColor = true;
            this.btnViewNewFont.Click += new System.EventHandler(this.btnViewNewFont_Click);
            // 
            // btnCopyOldFont
            // 
            this.btnCopyOldFont.Location = new System.Drawing.Point(6, 20);
            this.btnCopyOldFont.Name = "btnCopyOldFont";
            this.btnCopyOldFont.Size = new System.Drawing.Size(90, 25);
            this.btnCopyOldFont.TabIndex = 5;
            this.btnCopyOldFont.Text = "复制旧字库";
            this.btnCopyOldFont.UseVisualStyleBackColor = true;
            this.btnCopyOldFont.Click += new System.EventHandler(this.btnCopyOldFont_Click);
            // 
            // btnFontSelect
            // 
            this.btnFontSelect.Location = new System.Drawing.Point(8, 255);
            this.btnFontSelect.Name = "btnFontSelect";
            this.btnFontSelect.Size = new System.Drawing.Size(90, 25);
            this.btnFontSelect.TabIndex = 2;
            this.btnFontSelect.Text = "选择字体";
            this.btnFontSelect.UseVisualStyleBackColor = true;
            this.btnFontSelect.Click += new System.EventHandler(this.btnFontSelect_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCreateCharMap);
            this.groupBox1.Controls.Add(this.btnViewFontInfo);
            this.groupBox1.Controls.Add(this.btnViewOldFont);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(106, 267);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "旧字库操作";
            // 
            // btnCreateCharMap
            // 
            this.btnCreateCharMap.Location = new System.Drawing.Point(6, 82);
            this.btnCreateCharMap.Name = "btnCreateCharMap";
            this.btnCreateCharMap.Size = new System.Drawing.Size(90, 25);
            this.btnCreateCharMap.TabIndex = 8;
            this.btnCreateCharMap.Text = "生成字符映射";
            this.btnCreateCharMap.UseVisualStyleBackColor = true;
            this.btnCreateCharMap.Click += new System.EventHandler(this.btnCreateCharMap_Click);
            // 
            // btnViewFontInfo
            // 
            this.btnViewFontInfo.Location = new System.Drawing.Point(6, 51);
            this.btnViewFontInfo.Name = "btnViewFontInfo";
            this.btnViewFontInfo.Size = new System.Drawing.Size(90, 25);
            this.btnViewFontInfo.TabIndex = 7;
            this.btnViewFontInfo.Text = "查看具体信息";
            this.btnViewFontInfo.UseVisualStyleBackColor = true;
            this.btnViewFontInfo.Click += new System.EventHandler(this.btnViewFontInfo_Click);
            // 
            // btnViewOldFont
            // 
            this.btnViewOldFont.Location = new System.Drawing.Point(6, 20);
            this.btnViewOldFont.Name = "btnViewOldFont";
            this.btnViewOldFont.Size = new System.Drawing.Size(90, 25);
            this.btnViewOldFont.TabIndex = 6;
            this.btnViewOldFont.Text = "更换旧字库";
            this.btnViewOldFont.UseVisualStyleBackColor = true;
            this.btnViewOldFont.Click += new System.EventHandler(this.btnViewOldFont_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(510, 339);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "测试文字:";
            // 
            // txtFontTest
            // 
            this.txtFontTest.Font = new System.Drawing.Font("KaiTi", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFontTest.Location = new System.Drawing.Point(583, 325);
            this.txtFontTest.Name = "txtFontTest";
            this.txtFontTest.ReadOnly = true;
            this.txtFontTest.Size = new System.Drawing.Size(257, 35);
            this.txtFontTest.TabIndex = 0;
            this.txtFontTest.Text = "新字库字体选择";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.fontGrid);
            this.groupBox5.Location = new System.Drawing.Point(124, 13);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(602, 296);
            this.groupBox5.TabIndex = 22;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "字库信息";
            // 
            // fontGrid
            // 
            this.fontGrid.AllowDrop = true;
            this.fontGrid.AllowUserToAddRows = false;
            this.fontGrid.AllowUserToDeleteRows = false;
            this.fontGrid.AllowUserToResizeColumns = false;
            this.fontGrid.AllowUserToResizeRows = false;
            this.fontGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fontGrid.ColumnHeadersVisible = false;
            this.fontGrid.Location = new System.Drawing.Point(15, 18);
            this.fontGrid.Name = "fontGrid";
            this.fontGrid.RowHeadersVisible = false;
            this.fontGrid.RowTemplate.Height = 23;
            this.fontGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.fontGrid.ShowCellErrors = false;
            this.fontGrid.ShowRowErrors = false;
            this.fontGrid.Size = new System.Drawing.Size(575, 272);
            this.fontGrid.TabIndex = 7;
            this.fontGrid.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.fontGrid_CellMouseUp);
            this.fontGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.fontGrid_CellMouseDown);
            this.fontGrid.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.fontGrid_CellMouseMove);
            this.fontGrid.DragOver += new System.Windows.Forms.DragEventHandler(this.fontGrid_DragOver);
            this.fontGrid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.fontGrid_CellMouseDoubleClick);
            this.fontGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.fontGrid_DragDrop);
            // 
            // WiiFontEditer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 400);
            this.Controls.Add(this.panel1);
            this.Name = "WiiFontEditer";
            this.Text = "Wii字库做成";
            this.Controls.SetChildIndex(this.pnlTopBody, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fontGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtFontTest;
        private System.Windows.Forms.Button btnFontSelect;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Button btnCopyOldFont;
        private System.Windows.Forms.Button btnViewOldFont;
        private System.Windows.Forms.Button btnViewFontInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView fontGrid;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnViewNewFont;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCreateCharMap;
    }
}
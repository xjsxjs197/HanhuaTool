namespace Hanhua.TextEditTools.TxtresEdit
{
    partial class TresEditer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExp = new System.Windows.Forms.Button();
            this.btnHide = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnChgDecoder = new System.Windows.Forms.Button();
            this.ddlDecoder = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tresGrid = new System.Windows.Forms.DataGridView();
            this.StartPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TresText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HidText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.padding1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.padding2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NextOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.firstText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hidPadding1Text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.textBef = new System.Windows.Forms.ToolTip(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtEn = new System.Windows.Forms.RichTextBox();
            this.txtJp = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tresGrid)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExp);
            this.panel1.Controls.Add(this.btnHide);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnChgDecoder);
            this.panel1.Controls.Add(this.ddlDecoder);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(761, 42);
            this.panel1.TabIndex = 0;
            // 
            // btnExp
            // 
            this.btnExp.Location = new System.Drawing.Point(673, 10);
            this.btnExp.Name = "btnExp";
            this.btnExp.Size = new System.Drawing.Size(45, 22);
            this.btnExp.TabIndex = 12;
            this.btnExp.Text = "导出";
            this.btnExp.UseVisualStyleBackColor = true;
            this.btnExp.Click += new System.EventHandler(this.btnExp_Click);
            // 
            // btnHide
            // 
            this.btnHide.Location = new System.Drawing.Point(525, 10);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(63, 22);
            this.btnHide.TabIndex = 11;
            this.btnHide.Text = "显示所有";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(272, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(117, 19);
            this.txtSearch.TabIndex = 10;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(395, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(39, 22);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Enabled = false;
            this.btnCopy.Location = new System.Drawing.Point(594, 10);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(63, 22);
            this.btnCopy.TabIndex = 8;
            this.btnCopy.Text = "整理字库";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(453, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(66, 22);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "保存翻译";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnChgDecoder
            // 
            this.btnChgDecoder.Location = new System.Drawing.Point(136, 11);
            this.btnChgDecoder.Name = "btnChgDecoder";
            this.btnChgDecoder.Size = new System.Drawing.Size(85, 22);
            this.btnChgDecoder.TabIndex = 5;
            this.btnChgDecoder.Text = "表示";
            this.btnChgDecoder.UseVisualStyleBackColor = true;
            this.btnChgDecoder.Click += new System.EventHandler(this.btnChgDecoder_Click);
            // 
            // ddlDecoder
            // 
            this.ddlDecoder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDecoder.FormattingEnabled = true;
            this.ddlDecoder.Items.AddRange(new object[] {
            "Shift-Jis",
            "Utf-8",
            "JIS(20932)",
            "iso-2022-jp(50220)",
            "euc-jp(51932)"});
            this.ddlDecoder.Location = new System.Drawing.Point(12, 12);
            this.ddlDecoder.Name = "ddlDecoder";
            this.ddlDecoder.Size = new System.Drawing.Size(105, 20);
            this.ddlDecoder.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tresGrid);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(242, 42);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(519, 281);
            this.panel2.TabIndex = 1;
            // 
            // tresGrid
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tresGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tresGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tresGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StartPos,
            this.TresText,
            this.HidText,
            this.padding1,
            this.padding2,
            this.StartNum,
            this.NextOffset,
            this.firstText,
            this.hidPadding1Text,
            this.lineNumber,
            this.Column1});
            this.tresGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tresGrid.Location = new System.Drawing.Point(0, 0);
            this.tresGrid.Name = "tresGrid";
            this.tresGrid.RowHeadersVisible = false;
            this.tresGrid.RowTemplate.Height = 20;
            this.tresGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tresGrid.ShowCellErrors = false;
            this.tresGrid.ShowRowErrors = false;
            this.tresGrid.Size = new System.Drawing.Size(519, 281);
            this.tresGrid.TabIndex = 0;
            this.tresGrid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.tresGrid_CellBeginEdit);
            this.tresGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.tresGrid_CellEndEdit);
            this.tresGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tresGrid_CellClick);
            this.tresGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tresGrid_KeyDown);
            this.tresGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tresGrid_CellContentClick);
            // 
            // StartPos
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.StartPos.DefaultCellStyle = dataGridViewCellStyle2;
            this.StartPos.HeaderText = "开始位置";
            this.StartPos.Name = "StartPos";
            this.StartPos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StartPos.Visible = false;
            this.StartPos.Width = 80;
            // 
            // TresText
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.TresText.DefaultCellStyle = dataGridViewCellStyle3;
            this.TresText.HeaderText = "文本";
            this.TresText.Name = "TresText";
            this.TresText.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TresText.Width = 400;
            // 
            // HidText
            // 
            this.HidText.HeaderText = "隐藏文本";
            this.HidText.Name = "HidText";
            this.HidText.Visible = false;
            // 
            // padding1
            // 
            this.padding1.HeaderText = "不明1";
            this.padding1.Name = "padding1";
            this.padding1.ReadOnly = true;
            this.padding1.Visible = false;
            this.padding1.Width = 60;
            // 
            // padding2
            // 
            this.padding2.HeaderText = "几行";
            this.padding2.Name = "padding2";
            this.padding2.Width = 60;
            // 
            // StartNum
            // 
            this.StartNum.HeaderText = "Num";
            this.StartNum.Name = "StartNum";
            this.StartNum.ReadOnly = true;
            this.StartNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StartNum.Visible = false;
            this.StartNum.Width = 50;
            // 
            // NextOffset
            // 
            this.NextOffset.HeaderText = "下一个字符串位置";
            this.NextOffset.Name = "NextOffset";
            this.NextOffset.Visible = false;
            // 
            // firstText
            // 
            this.firstText.HeaderText = "隐藏文本1";
            this.firstText.Name = "firstText";
            this.firstText.Visible = false;
            // 
            // hidPadding1Text
            // 
            this.hidPadding1Text.HeaderText = "隐藏文本2";
            this.hidPadding1Text.Name = "hidPadding1Text";
            this.hidPadding1Text.Visible = false;
            // 
            // lineNumber
            // 
            this.lineNumber.HeaderText = "隐藏行数";
            this.lineNumber.Name = "lineNumber";
            this.lineNumber.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.lineNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.lineNumber.Visible = false;
            this.lineNumber.Width = 50;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "完成";
            this.Column1.Name = "Column1";
            this.Column1.Width = 50;
            // 
            // textBef
            // 
            this.textBef.BackColor = System.Drawing.Color.SandyBrown;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtEn);
            this.panel3.Controls.Add(this.txtJp);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 42);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(242, 281);
            this.panel3.TabIndex = 2;
            // 
            // txtEn
            // 
            this.txtEn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtEn.Location = new System.Drawing.Point(0, 132);
            this.txtEn.Name = "txtEn";
            this.txtEn.ReadOnly = true;
            this.txtEn.Size = new System.Drawing.Size(242, 149);
            this.txtEn.TabIndex = 2;
            this.txtEn.Text = "";
            // 
            // txtJp
            // 
            this.txtJp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJp.Location = new System.Drawing.Point(0, 0);
            this.txtJp.Name = "txtJp";
            this.txtJp.ReadOnly = true;
            this.txtJp.Size = new System.Drawing.Size(242, 281);
            this.txtJp.TabIndex = 1;
            this.txtJp.Text = "";
            // 
            // TresEditer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 348);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "TresEditer";
            this.Text = "Tres编辑";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tresGrid)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView tresGrid;
        private System.Windows.Forms.ComboBox ddlDecoder;
        private System.Windows.Forms.Button btnChgDecoder;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolTip textBef;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RichTextBox txtEn;
        private System.Windows.Forms.RichTextBox txtJp;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn TresText;
        private System.Windows.Forms.DataGridViewTextBoxColumn HidText;
        private System.Windows.Forms.DataGridViewTextBoxColumn padding1;
        private System.Windows.Forms.DataGridViewTextBoxColumn padding2;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn NextOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn firstText;
        private System.Windows.Forms.DataGridViewTextBoxColumn hidPadding1Text;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineNumber;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.Button btnExp;
    }
}
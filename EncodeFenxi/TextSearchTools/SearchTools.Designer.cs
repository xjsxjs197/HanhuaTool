namespace Hanhua.Common
{
    partial class Fenxi
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnStartFenxi = new System.Windows.Forms.Button();
            this.btnMuluFenxi = new System.Windows.Forms.Button();
            this.grbCondition = new System.Windows.Forms.GroupBox();
            this.rdoToByte = new System.Windows.Forms.RadioButton();
            this.rdoToPos = new System.Windows.Forms.RadioButton();
            this.cmbTxtToByte = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnTextChg = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboSpecial = new System.Windows.Forms.ComboBox();
            this.rdoSpecial = new System.Windows.Forms.RadioButton();
            this.rdoDiffSearch2 = new System.Windows.Forms.RadioButton();
            this.rdoDiffSearch = new System.Windows.Forms.RadioButton();
            this.rdoBin = new System.Windows.Forms.RadioButton();
            this.rdoText = new System.Windows.Forms.RadioButton();
            this.txtKeyWord = new System.Windows.Forms.TextBox();
            this.lblKeyWord = new System.Windows.Forms.Label();
            this.gbxDecoder = new System.Windows.Forms.GroupBox();
            this.chk10001 = new System.Windows.Forms.CheckBox();
            this.chk50221 = new System.Windows.Forms.CheckBox();
            this.chk50222 = new System.Windows.Forms.CheckBox();
            this.cmbUnicodeType = new System.Windows.Forms.ComboBox();
            this.chk51932 = new System.Windows.Forms.CheckBox();
            this.chk50220 = new System.Windows.Forms.CheckBox();
            this.chk20932 = new System.Windows.Forms.CheckBox();
            this.chkUtf8 = new System.Windows.Forms.CheckBox();
            this.chkUnicode = new System.Windows.Forms.CheckBox();
            this.chkShiftJis = new System.Windows.Forms.CheckBox();
            this.gridSearchResult = new System.Windows.Forms.DataGridView();
            this.hidFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.content = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.decodeIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnReCheck = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnLook = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdoChgStr = new System.Windows.Forms.RadioButton();
            this.grbCondition.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbxDecoder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSearchResult)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Size = new System.Drawing.Size(847, 404);
            // 
            // btnStartFenxi
            // 
            this.btnStartFenxi.Location = new System.Drawing.Point(8, 18);
            this.btnStartFenxi.Name = "btnStartFenxi";
            this.btnStartFenxi.Size = new System.Drawing.Size(99, 29);
            this.btnStartFenxi.TabIndex = 0;
            this.btnStartFenxi.Text = "单个文件分析";
            this.btnStartFenxi.UseVisualStyleBackColor = true;
            this.btnStartFenxi.Click += new System.EventHandler(this.btnStartFenxi_Click);
            // 
            // btnMuluFenxi
            // 
            this.btnMuluFenxi.Location = new System.Drawing.Point(8, 53);
            this.btnMuluFenxi.Name = "btnMuluFenxi";
            this.btnMuluFenxi.Size = new System.Drawing.Size(99, 29);
            this.btnMuluFenxi.TabIndex = 2;
            this.btnMuluFenxi.Text = "目录分析";
            this.btnMuluFenxi.UseVisualStyleBackColor = true;
            this.btnMuluFenxi.Click += new System.EventHandler(this.btnMuluFenxi_Click);
            // 
            // grbCondition
            // 
            this.grbCondition.Controls.Add(this.rdoChgStr);
            this.grbCondition.Controls.Add(this.rdoToByte);
            this.grbCondition.Controls.Add(this.rdoToPos);
            this.grbCondition.Controls.Add(this.cmbTxtToByte);
            this.grbCondition.Controls.Add(this.label1);
            this.grbCondition.Controls.Add(this.btnTextChg);
            this.grbCondition.Controls.Add(this.groupBox1);
            this.grbCondition.Controls.Add(this.txtKeyWord);
            this.grbCondition.Controls.Add(this.lblKeyWord);
            this.grbCondition.Location = new System.Drawing.Point(398, 7);
            this.grbCondition.Name = "grbCondition";
            this.grbCondition.Size = new System.Drawing.Size(439, 220);
            this.grbCondition.TabIndex = 3;
            this.grbCondition.TabStop = false;
            this.grbCondition.Text = "条件过滤";
            // 
            // rdoToByte
            // 
            this.rdoToByte.AutoSize = true;
            this.rdoToByte.Location = new System.Drawing.Point(257, 90);
            this.rdoToByte.Name = "rdoToByte";
            this.rdoToByte.Size = new System.Drawing.Size(71, 16);
            this.rdoToByte.TabIndex = 13;
            this.rdoToByte.Text = "转二进制";
            this.rdoToByte.UseVisualStyleBackColor = true;
            this.rdoToByte.CheckedChanged += new System.EventHandler(this.rdoToByte_CheckedChanged);
            // 
            // rdoToPos
            // 
            this.rdoToPos.AutoSize = true;
            this.rdoToPos.Checked = true;
            this.rdoToPos.Location = new System.Drawing.Point(196, 90);
            this.rdoToPos.Name = "rdoToPos";
            this.rdoToPos.Size = new System.Drawing.Size(59, 16);
            this.rdoToPos.TabIndex = 12;
            this.rdoToPos.TabStop = true;
            this.rdoToPos.Text = "转位置";
            this.rdoToPos.UseVisualStyleBackColor = true;
            // 
            // cmbTxtToByte
            // 
            this.cmbTxtToByte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTxtToByte.FormattingEnabled = true;
            this.cmbTxtToByte.Items.AddRange(new object[] {
            "通常编码（Shift-Jis）",
            "通常编码（Utf-8）",
            "生化1文件文本编码",
            "生化1通常文本编码",
            "红侠乔伊文本编码",
            "通常编码（Unicode Big end）",
            "通常编码（Unicode Little end）"});
            this.cmbTxtToByte.Location = new System.Drawing.Point(186, 57);
            this.cmbTxtToByte.Name = "cmbTxtToByte";
            this.cmbTxtToByte.Size = new System.Drawing.Size(159, 20);
            this.cmbTxtToByte.TabIndex = 11;
            this.cmbTxtToByte.SelectedIndexChanged += new System.EventHandler(this.cmbTxtToByte_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "文字转换协助";
            // 
            // btnTextChg
            // 
            this.btnTextChg.Location = new System.Drawing.Point(364, 55);
            this.btnTextChg.Name = "btnTextChg";
            this.btnTextChg.Size = new System.Drawing.Size(64, 23);
            this.btnTextChg.TabIndex = 9;
            this.btnTextChg.Text = "开始转换";
            this.btnTextChg.UseVisualStyleBackColor = true;
            this.btnTextChg.Click += new System.EventHandler(this.btnTextChg_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboSpecial);
            this.groupBox1.Controls.Add(this.rdoSpecial);
            this.groupBox1.Controls.Add(this.rdoDiffSearch2);
            this.groupBox1.Controls.Add(this.rdoDiffSearch);
            this.groupBox1.Controls.Add(this.rdoBin);
            this.groupBox1.Controls.Add(this.rdoText);
            this.groupBox1.Location = new System.Drawing.Point(13, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(415, 98);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查找方式";
            // 
            // cboSpecial
            // 
            this.cboSpecial.FormattingEnabled = true;
            this.cboSpecial.Items.AddRange(new object[] {
            "Tpl格式图片",
            "REFF格式图片",
            "REFT格式图片",
            "TEX0格式图片",
            "Wii字库",
            "NGC字库",
            "RARC文件",
            "Uｪ8-文件",
            "Yaz0压缩格式文件"});
            this.cboSpecial.Location = new System.Drawing.Point(139, 64);
            this.cboSpecial.Name = "cboSpecial";
            this.cboSpecial.Size = new System.Drawing.Size(120, 20);
            this.cboSpecial.TabIndex = 5;
            this.cboSpecial.SelectedIndexChanged += new System.EventHandler(this.cboSpecial_SelectedIndexChanged);
            // 
            // rdoSpecial
            // 
            this.rdoSpecial.AutoSize = true;
            this.rdoSpecial.Location = new System.Drawing.Point(38, 65);
            this.rdoSpecial.Name = "rdoSpecial";
            this.rdoSpecial.Size = new System.Drawing.Size(95, 16);
            this.rdoSpecial.TabIndex = 4;
            this.rdoSpecial.Text = "特定格式查找";
            this.rdoSpecial.UseVisualStyleBackColor = true;
            this.rdoSpecial.CheckedChanged += new System.EventHandler(this.rdoSpecial_CheckedChanged);
            // 
            // rdoDiffSearch2
            // 
            this.rdoDiffSearch2.AutoSize = true;
            this.rdoDiffSearch2.Checked = true;
            this.rdoDiffSearch2.Location = new System.Drawing.Point(257, 39);
            this.rdoDiffSearch2.Name = "rdoDiffSearch2";
            this.rdoDiffSearch2.Size = new System.Drawing.Size(139, 16);
            this.rdoDiffSearch2.TabIndex = 3;
            this.rdoDiffSearch2.TabStop = true;
            this.rdoDiffSearch2.Text = "差值格式查找(双字节)";
            this.rdoDiffSearch2.UseVisualStyleBackColor = true;
            // 
            // rdoDiffSearch
            // 
            this.rdoDiffSearch.AutoSize = true;
            this.rdoDiffSearch.Location = new System.Drawing.Point(257, 17);
            this.rdoDiffSearch.Name = "rdoDiffSearch";
            this.rdoDiffSearch.Size = new System.Drawing.Size(139, 16);
            this.rdoDiffSearch.TabIndex = 2;
            this.rdoDiffSearch.Text = "差值格式查找(单字节)";
            this.rdoDiffSearch.UseVisualStyleBackColor = true;
            this.rdoDiffSearch.CheckedChanged += new System.EventHandler(this.rdoDiffSearch_CheckedChanged);
            // 
            // rdoBin
            // 
            this.rdoBin.AutoSize = true;
            this.rdoBin.Location = new System.Drawing.Point(144, 17);
            this.rdoBin.Name = "rdoBin";
            this.rdoBin.Size = new System.Drawing.Size(107, 16);
            this.rdoBin.TabIndex = 1;
            this.rdoBin.Text = "二进制格式查找";
            this.rdoBin.UseVisualStyleBackColor = true;
            this.rdoBin.CheckedChanged += new System.EventHandler(this.rdoBin_CheckedChanged);
            // 
            // rdoText
            // 
            this.rdoText.AutoSize = true;
            this.rdoText.Location = new System.Drawing.Point(38, 17);
            this.rdoText.Name = "rdoText";
            this.rdoText.Size = new System.Drawing.Size(95, 16);
            this.rdoText.TabIndex = 0;
            this.rdoText.Text = "文本格式查找";
            this.rdoText.UseVisualStyleBackColor = true;
            // 
            // txtKeyWord
            // 
            this.txtKeyWord.Location = new System.Drawing.Point(186, 25);
            this.txtKeyWord.Name = "txtKeyWord";
            this.txtKeyWord.Size = new System.Drawing.Size(242, 19);
            this.txtKeyWord.TabIndex = 1;
            this.txtKeyWord.Text = "61 62 85 86 93 94 254 89";
            // 
            // lblKeyWord
            // 
            this.lblKeyWord.AutoSize = true;
            this.lblKeyWord.Location = new System.Drawing.Point(19, 28);
            this.lblKeyWord.Name = "lblKeyWord";
            this.lblKeyWord.Size = new System.Drawing.Size(133, 12);
            this.lblKeyWord.TabIndex = 0;
            this.lblKeyWord.Text = "查找的关键字(逗号分隔)";
            // 
            // gbxDecoder
            // 
            this.gbxDecoder.Controls.Add(this.chk10001);
            this.gbxDecoder.Controls.Add(this.chk50221);
            this.gbxDecoder.Controls.Add(this.chk50222);
            this.gbxDecoder.Controls.Add(this.cmbUnicodeType);
            this.gbxDecoder.Controls.Add(this.chk51932);
            this.gbxDecoder.Controls.Add(this.chk50220);
            this.gbxDecoder.Controls.Add(this.chk20932);
            this.gbxDecoder.Controls.Add(this.chkUtf8);
            this.gbxDecoder.Controls.Add(this.chkUnicode);
            this.gbxDecoder.Controls.Add(this.chkShiftJis);
            this.gbxDecoder.Location = new System.Drawing.Point(113, 7);
            this.gbxDecoder.Name = "gbxDecoder";
            this.gbxDecoder.Size = new System.Drawing.Size(279, 220);
            this.gbxDecoder.TabIndex = 4;
            this.gbxDecoder.TabStop = false;
            this.gbxDecoder.Text = "解码方式选择";
            // 
            // chk10001
            // 
            this.chk10001.AutoSize = true;
            this.chk10001.Location = new System.Drawing.Point(15, 194);
            this.chk10001.Name = "chk10001";
            this.chk10001.Size = new System.Drawing.Size(96, 16);
            this.chk10001.TabIndex = 9;
            this.chk10001.Text = "Mac_jp(50222)";
            this.chk10001.UseVisualStyleBackColor = true;
            // 
            // chk50221
            // 
            this.chk50221.AutoSize = true;
            this.chk50221.Location = new System.Drawing.Point(15, 150);
            this.chk50221.Name = "chk50221";
            this.chk50221.Size = new System.Drawing.Size(181, 16);
            this.chk50221.TabIndex = 8;
            this.chk50221.Text = "Jis1 Allow 1 byte Kana(50221)";
            this.chk50221.UseVisualStyleBackColor = true;
            // 
            // chk50222
            // 
            this.chk50222.AutoSize = true;
            this.chk50222.Location = new System.Drawing.Point(15, 172);
            this.chk50222.Name = "chk50222";
            this.chk50222.Size = new System.Drawing.Size(226, 16);
            this.chk50222.TabIndex = 7;
            this.chk50222.Text = "Jis1 Allow 1 byte Kana - SO/SI(50222)";
            this.chk50222.UseVisualStyleBackColor = true;
            // 
            // cmbUnicodeType
            // 
            this.cmbUnicodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnicodeType.FormattingEnabled = true;
            this.cmbUnicodeType.Items.AddRange(new object[] {
            "Big Endian",
            "Little Endian"});
            this.cmbUnicodeType.Location = new System.Drawing.Point(105, 82);
            this.cmbUnicodeType.Name = "cmbUnicodeType";
            this.cmbUnicodeType.Size = new System.Drawing.Size(72, 20);
            this.cmbUnicodeType.TabIndex = 6;
            // 
            // chk51932
            // 
            this.chk51932.AutoSize = true;
            this.chk51932.Location = new System.Drawing.Point(15, 106);
            this.chk51932.Name = "chk51932";
            this.chk51932.Size = new System.Drawing.Size(95, 16);
            this.chk51932.TabIndex = 5;
            this.chk51932.Text = "euc-jp(51932)";
            this.chk51932.UseVisualStyleBackColor = true;
            // 
            // chk50220
            // 
            this.chk50220.AutoSize = true;
            this.chk50220.Location = new System.Drawing.Point(15, 128);
            this.chk50220.Name = "chk50220";
            this.chk50220.Size = new System.Drawing.Size(122, 16);
            this.chk50220.TabIndex = 4;
            this.chk50220.Text = "iso-2022-jp(50220)";
            this.chk50220.UseVisualStyleBackColor = true;
            // 
            // chk20932
            // 
            this.chk20932.AutoSize = true;
            this.chk20932.Location = new System.Drawing.Point(15, 62);
            this.chk20932.Name = "chk20932";
            this.chk20932.Size = new System.Drawing.Size(79, 16);
            this.chk20932.TabIndex = 3;
            this.chk20932.Text = "JIS(20932)";
            this.chk20932.UseVisualStyleBackColor = true;
            // 
            // chkUtf8
            // 
            this.chkUtf8.AutoSize = true;
            this.chkUtf8.Location = new System.Drawing.Point(15, 40);
            this.chkUtf8.Name = "chkUtf8";
            this.chkUtf8.Size = new System.Drawing.Size(52, 16);
            this.chkUtf8.TabIndex = 2;
            this.chkUtf8.Text = "Utf-8";
            this.chkUtf8.UseVisualStyleBackColor = true;
            // 
            // chkUnicode
            // 
            this.chkUnicode.AutoSize = true;
            this.chkUnicode.Location = new System.Drawing.Point(15, 84);
            this.chkUnicode.Name = "chkUnicode";
            this.chkUnicode.Size = new System.Drawing.Size(65, 16);
            this.chkUnicode.TabIndex = 1;
            this.chkUnicode.Text = "Unicode";
            this.chkUnicode.UseVisualStyleBackColor = true;
            // 
            // chkShiftJis
            // 
            this.chkShiftJis.AutoSize = true;
            this.chkShiftJis.Checked = true;
            this.chkShiftJis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShiftJis.Location = new System.Drawing.Point(15, 18);
            this.chkShiftJis.Name = "chkShiftJis";
            this.chkShiftJis.Size = new System.Drawing.Size(96, 16);
            this.chkShiftJis.TabIndex = 0;
            this.chkShiftJis.Text = "Shift-Jis(932)";
            this.chkShiftJis.UseVisualStyleBackColor = true;
            // 
            // gridSearchResult
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSearchResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridSearchResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSearchResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.hidFileName,
            this.fileName,
            this.startPos,
            this.content,
            this.endPos,
            this.decodeIndex,
            this.btnReCheck});
            this.gridSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSearchResult.Location = new System.Drawing.Point(0, 0);
            this.gridSearchResult.Name = "gridSearchResult";
            this.gridSearchResult.RowHeadersVisible = false;
            this.gridSearchResult.RowTemplate.Height = 21;
            this.gridSearchResult.Size = new System.Drawing.Size(847, 171);
            this.gridSearchResult.TabIndex = 5;
            // 
            // hidFileName
            // 
            this.hidFileName.HeaderText = "隐藏文件名";
            this.hidFileName.Name = "hidFileName";
            this.hidFileName.Visible = false;
            // 
            // fileName
            // 
            this.fileName.DataPropertyName = "fileName";
            this.fileName.HeaderText = "文件名";
            this.fileName.Name = "fileName";
            this.fileName.Width = 150;
            // 
            // startPos
            // 
            this.startPos.DataPropertyName = "startPos";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.startPos.DefaultCellStyle = dataGridViewCellStyle2;
            this.startPos.HeaderText = "位置";
            this.startPos.Name = "startPos";
            this.startPos.Width = 150;
            // 
            // content
            // 
            this.content.DataPropertyName = "content";
            this.content.HeaderText = "内容";
            this.content.Name = "content";
            this.content.Width = 400;
            // 
            // endPos
            // 
            this.endPos.DataPropertyName = "endPos";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.endPos.DefaultCellStyle = dataGridViewCellStyle3;
            this.endPos.HeaderText = "编码方式";
            this.endPos.Name = "endPos";
            this.endPos.Width = 80;
            // 
            // decodeIndex
            // 
            this.decodeIndex.HeaderText = "解码器Index";
            this.decodeIndex.Name = "decodeIndex";
            this.decodeIndex.Visible = false;
            // 
            // btnReCheck
            // 
            this.btnReCheck.HeaderText = "再分析";
            this.btnReCheck.Name = "btnReCheck";
            this.btnReCheck.Width = 60;
            // 
            // btnLook
            // 
            this.btnLook.Location = new System.Drawing.Point(8, 113);
            this.btnLook.Name = "btnLook";
            this.btnLook.Size = new System.Drawing.Size(99, 29);
            this.btnLook.TabIndex = 6;
            this.btnLook.Text = "文本查看工具";
            this.btnLook.UseVisualStyleBackColor = true;
            this.btnLook.Click += new System.EventHandler(this.btnLook_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnLook);
            this.panel1.Controls.Add(this.gbxDecoder);
            this.panel1.Controls.Add(this.grbCondition);
            this.panel1.Controls.Add(this.btnMuluFenxi);
            this.panel1.Controls.Add(this.btnStartFenxi);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(847, 233);
            this.panel1.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridSearchResult);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 233);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(847, 171);
            this.panel2.TabIndex = 8;
            // 
            // rdoChgStr
            // 
            this.rdoChgStr.AutoSize = true;
            this.rdoChgStr.Location = new System.Drawing.Point(334, 90);
            this.rdoChgStr.Name = "rdoChgStr";
            this.rdoChgStr.Size = new System.Drawing.Size(59, 16);
            this.rdoChgStr.TabIndex = 14;
            this.rdoChgStr.Text = "转文字";
            this.rdoChgStr.UseVisualStyleBackColor = true;
            // 
            // Fenxi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 429);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Fenxi";
            this.Text = "查找工具";
            this.Controls.SetChildIndex(this.pnlTopBody, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.grbCondition.ResumeLayout(false);
            this.grbCondition.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbxDecoder.ResumeLayout(false);
            this.gbxDecoder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSearchResult)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartFenxi;
        private System.Windows.Forms.Button btnMuluFenxi;
        private System.Windows.Forms.GroupBox grbCondition;
        private System.Windows.Forms.Label lblKeyWord;
        private System.Windows.Forms.TextBox txtKeyWord;
        private System.Windows.Forms.GroupBox gbxDecoder;
        private System.Windows.Forms.CheckBox chkShiftJis;
        private System.Windows.Forms.CheckBox chkUnicode;
        private System.Windows.Forms.CheckBox chkUtf8;
        private System.Windows.Forms.CheckBox chk20932;
        private System.Windows.Forms.CheckBox chk50220;
        private System.Windows.Forms.CheckBox chk51932;
        private System.Windows.Forms.DataGridView gridSearchResult;
        private System.Windows.Forms.Button btnLook;
        private System.Windows.Forms.ComboBox cmbUnicodeType;
        private System.Windows.Forms.CheckBox chk50221;
        private System.Windows.Forms.CheckBox chk50222;
        private System.Windows.Forms.CheckBox chk10001;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoText;
        private System.Windows.Forms.RadioButton rdoBin;
        private System.Windows.Forms.DataGridViewTextBoxColumn hidFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn startPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn content;
        private System.Windows.Forms.DataGridViewTextBoxColumn endPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn decodeIndex;
        private System.Windows.Forms.DataGridViewButtonColumn btnReCheck;
        private System.Windows.Forms.RadioButton rdoDiffSearch;
        private System.Windows.Forms.RadioButton rdoDiffSearch2;
        private System.Windows.Forms.RadioButton rdoSpecial;
        private System.Windows.Forms.ComboBox cboSpecial;
        private System.Windows.Forms.Button btnTextChg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTxtToByte;
        private System.Windows.Forms.RadioButton rdoToPos;
        private System.Windows.Forms.RadioButton rdoToByte;
        private System.Windows.Forms.RadioButton rdoChgStr;
    }
}


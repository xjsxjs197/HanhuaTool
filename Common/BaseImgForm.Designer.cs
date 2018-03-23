namespace Hanhua.Common
{
    partial class BaseImgForm
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
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lstFont = new System.Windows.Forms.ListBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.txtBlockH = new System.Windows.Forms.TextBox();
            this.txtBlockW = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFontSize = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSample = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnReDraw = new System.Windows.Forms.Button();
            this.txtYPadding = new System.Windows.Forms.TextBox();
            this.txtXPadding = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkNeedBorder = new System.Windows.Forms.CheckBox();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.pnlImg = new System.Windows.Forms.Panel();
            this.grdBodyImg = new System.Windows.Forms.DataGridView();
            this.pnlSample = new System.Windows.Forms.Panel();
            this.grdSampleImg = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewImageColumn();
            this.lblSample = new System.Windows.Forms.Label();
            this.pnlTopBody.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlImg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdBodyImg)).BeginInit();
            this.pnlSample.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSampleImg)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Controls.Add(this.pnlBody);
            this.pnlTopBody.Controls.Add(this.pnlLeft);
            this.pnlTopBody.Controls.Add(this.pnlBottom);
            this.pnlTopBody.Size = new System.Drawing.Size(715, 373);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.lstFont);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(115, 312);
            this.pnlLeft.TabIndex = 0;
            // 
            // lstFont
            // 
            this.lstFont.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFont.FormattingEnabled = true;
            this.lstFont.ItemHeight = 12;
            this.lstFont.Location = new System.Drawing.Point(0, 0);
            this.lstFont.Name = "lstFont";
            this.lstFont.Size = new System.Drawing.Size(115, 304);
            this.lstFont.TabIndex = 0;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.pnlBottom.Controls.Add(this.txtBlockH);
            this.pnlBottom.Controls.Add(this.txtBlockW);
            this.pnlBottom.Controls.Add(this.label5);
            this.pnlBottom.Controls.Add(this.label6);
            this.pnlBottom.Controls.Add(this.txtFontSize);
            this.pnlBottom.Controls.Add(this.label4);
            this.pnlBottom.Controls.Add(this.txtSample);
            this.pnlBottom.Controls.Add(this.label3);
            this.pnlBottom.Controls.Add(this.btnReDraw);
            this.pnlBottom.Controls.Add(this.txtYPadding);
            this.pnlBottom.Controls.Add(this.txtXPadding);
            this.pnlBottom.Controls.Add(this.label2);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Controls.Add(this.chkNeedBorder);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 312);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(715, 61);
            this.pnlBottom.TabIndex = 1;
            // 
            // txtBlockH
            // 
            this.txtBlockH.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBlockH.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtBlockH.Location = new System.Drawing.Point(280, 33);
            this.txtBlockH.Name = "txtBlockH";
            this.txtBlockH.Size = new System.Drawing.Size(30, 19);
            this.txtBlockH.TabIndex = 13;
            this.txtBlockH.Text = "32";
            this.txtBlockH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtBlockW
            // 
            this.txtBlockW.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBlockW.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtBlockW.Location = new System.Drawing.Point(280, 9);
            this.txtBlockW.Name = "txtBlockW";
            this.txtBlockW.Size = new System.Drawing.Size(30, 19);
            this.txtBlockW.TabIndex = 12;
            this.txtBlockW.Text = "32";
            this.txtBlockW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(225, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "BlockH：";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(225, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "BlockW：";
            // 
            // txtFontSize
            // 
            this.txtFontSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.txtFontSize.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtFontSize.Location = new System.Drawing.Point(77, 33);
            this.txtFontSize.Name = "txtFontSize";
            this.txtFontSize.Size = new System.Drawing.Size(30, 19);
            this.txtFontSize.TabIndex = 9;
            this.txtFontSize.Text = "20";
            this.txtFontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "字体大小：";
            // 
            // txtSample
            // 
            this.txtSample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSample.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtSample.Location = new System.Drawing.Point(400, 33);
            this.txtSample.Name = "txtSample";
            this.txtSample.Size = new System.Drawing.Size(124, 19);
            this.txtSample.TabIndex = 7;
            this.txtSample.Text = "这是测试文字012！";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(335, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "事例文字：";
            // 
            // btnReDraw
            // 
            this.btnReDraw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReDraw.Location = new System.Drawing.Point(625, 8);
            this.btnReDraw.Name = "btnReDraw";
            this.btnReDraw.Size = new System.Drawing.Size(78, 32);
            this.btnReDraw.TabIndex = 5;
            this.btnReDraw.Text = "刷新";
            this.btnReDraw.UseVisualStyleBackColor = true;
            this.btnReDraw.Click += new System.EventHandler(this.btnReDraw_Click);
            // 
            // txtYPadding
            // 
            this.txtYPadding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.txtYPadding.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtYPadding.Location = new System.Drawing.Point(180, 33);
            this.txtYPadding.Name = "txtYPadding";
            this.txtYPadding.Size = new System.Drawing.Size(30, 19);
            this.txtYPadding.TabIndex = 4;
            this.txtYPadding.Text = "2";
            this.txtYPadding.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtXPadding
            // 
            this.txtXPadding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.txtXPadding.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtXPadding.Location = new System.Drawing.Point(180, 9);
            this.txtXPadding.Name = "txtXPadding";
            this.txtXPadding.Size = new System.Drawing.Size(30, 19);
            this.txtXPadding.TabIndex = 3;
            this.txtXPadding.Text = "2";
            this.txtXPadding.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(116, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "YPadding：";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(116, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "XPadding：";
            // 
            // chkNeedBorder
            // 
            this.chkNeedBorder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.chkNeedBorder.AutoSize = true;
            this.chkNeedBorder.Checked = true;
            this.chkNeedBorder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNeedBorder.Location = new System.Drawing.Point(12, 11);
            this.chkNeedBorder.Name = "chkNeedBorder";
            this.chkNeedBorder.Size = new System.Drawing.Size(72, 16);
            this.chkNeedBorder.TabIndex = 0;
            this.chkNeedBorder.Text = "是否描边";
            this.chkNeedBorder.UseVisualStyleBackColor = true;
            this.chkNeedBorder.CheckedChanged += new System.EventHandler(this.chkNeedBorder_CheckedChanged);
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.pnlImg);
            this.pnlBody.Controls.Add(this.pnlSample);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(115, 0);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(600, 312);
            this.pnlBody.TabIndex = 2;
            // 
            // pnlImg
            // 
            this.pnlImg.Controls.Add(this.grdBodyImg);
            this.pnlImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImg.Location = new System.Drawing.Point(0, 50);
            this.pnlImg.Name = "pnlImg";
            this.pnlImg.Size = new System.Drawing.Size(600, 262);
            this.pnlImg.TabIndex = 3;
            // 
            // grdBodyImg
            // 
            this.grdBodyImg.AllowUserToAddRows = false;
            this.grdBodyImg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdBodyImg.ColumnHeadersVisible = false;
            this.grdBodyImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdBodyImg.Location = new System.Drawing.Point(0, 0);
            this.grdBodyImg.Name = "grdBodyImg";
            this.grdBodyImg.RowTemplate.Height = 21;
            this.grdBodyImg.ShowCellErrors = false;
            this.grdBodyImg.ShowRowErrors = false;
            this.grdBodyImg.Size = new System.Drawing.Size(600, 262);
            this.grdBodyImg.TabIndex = 2;
            // 
            // pnlSample
            // 
            this.pnlSample.Controls.Add(this.grdSampleImg);
            this.pnlSample.Controls.Add(this.lblSample);
            this.pnlSample.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSample.Location = new System.Drawing.Point(0, 0);
            this.pnlSample.Name = "pnlSample";
            this.pnlSample.Size = new System.Drawing.Size(600, 50);
            this.pnlSample.TabIndex = 2;
            // 
            // grdSampleImg
            // 
            this.grdSampleImg.AllowUserToAddRows = false;
            this.grdSampleImg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSampleImg.ColumnHeadersVisible = false;
            this.grdSampleImg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10});
            this.grdSampleImg.Dock = System.Windows.Forms.DockStyle.Right;
            this.grdSampleImg.Location = new System.Drawing.Point(100, 0);
            this.grdSampleImg.Name = "grdSampleImg";
            this.grdSampleImg.RowHeadersVisible = false;
            this.grdSampleImg.RowTemplate.Height = 21;
            this.grdSampleImg.ShowCellErrors = false;
            this.grdSampleImg.ShowRowErrors = false;
            this.grdSampleImg.Size = new System.Drawing.Size(500, 50);
            this.grdSampleImg.TabIndex = 1;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.Width = 32;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.Width = 32;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            this.Column3.Width = 32;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Column4";
            this.Column4.Name = "Column4";
            this.Column4.Width = 32;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Column5";
            this.Column5.Name = "Column5";
            this.Column5.Width = 32;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Column6";
            this.Column6.Name = "Column6";
            this.Column6.Width = 32;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Column7";
            this.Column7.Name = "Column7";
            this.Column7.Width = 32;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Column8";
            this.Column8.Name = "Column8";
            this.Column8.Width = 32;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Column9";
            this.Column9.Name = "Column9";
            this.Column9.Width = 32;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Column10";
            this.Column10.Name = "Column10";
            this.Column10.Width = 32;
            // 
            // lblSample
            // 
            this.lblSample.AutoSize = true;
            this.lblSample.Location = new System.Drawing.Point(26, 18);
            this.lblSample.Name = "lblSample";
            this.lblSample.Size = new System.Drawing.Size(55, 12);
            this.lblSample.TabIndex = 0;
            this.lblSample.Text = "事例图片:";
            // 
            // BaseImgForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 398);
            this.Name = "BaseImgForm";
            this.Text = "图片处理共通";
            this.pnlTopBody.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.pnlImg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdBodyImg)).EndInit();
            this.pnlSample.ResumeLayout(false);
            this.pnlSample.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSampleImg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.ListBox lstFont;
        private System.Windows.Forms.Label lblSample;
        private System.Windows.Forms.CheckBox chkNeedBorder;
        private System.Windows.Forms.TextBox txtXPadding;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtYPadding;
        private System.Windows.Forms.Button btnReDraw;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSample;
        private System.Windows.Forms.TextBox txtFontSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlImg;
        private System.Windows.Forms.Panel pnlSample;
        private System.Windows.Forms.DataGridView grdSampleImg;
        private System.Windows.Forms.DataGridView grdBodyImg;
        private System.Windows.Forms.TextBox txtBlockH;
        private System.Windows.Forms.TextBox txtBlockW;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewImageColumn Column1;
        private System.Windows.Forms.DataGridViewImageColumn Column2;
        private System.Windows.Forms.DataGridViewImageColumn Column3;
        private System.Windows.Forms.DataGridViewImageColumn Column4;
        private System.Windows.Forms.DataGridViewImageColumn Column5;
        private System.Windows.Forms.DataGridViewImageColumn Column6;
        private System.Windows.Forms.DataGridViewImageColumn Column7;
        private System.Windows.Forms.DataGridViewImageColumn Column8;
        private System.Windows.Forms.DataGridViewImageColumn Column9;
        private System.Windows.Forms.DataGridViewImageColumn Column10;
    }
}
namespace Hanhua.TextEditTools.ViewtifulJoe
{
    partial class ViewtifulJoePicEditor
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
            this.lstPic = new System.Windows.Forms.ListBox();
            this.imgJp = new System.Windows.Forms.PictureBox();
            this.imgCn = new System.Windows.Forms.PictureBox();
            this.txtCn = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnChgFont = new System.Windows.Forms.Button();
            this.btnFontD = new System.Windows.Forms.Button();
            this.btnFontA = new System.Windows.Forms.Button();
            this.txtMoveRight = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReload = new System.Windows.Forms.Button();
            this.pnlTopBody.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgJp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCn)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(237)))), ((int)(((byte)(204)))));
            this.pnlTopBody.Controls.Add(this.btnReload);
            this.pnlTopBody.Controls.Add(this.label1);
            this.pnlTopBody.Controls.Add(this.txtMoveRight);
            this.pnlTopBody.Controls.Add(this.btnFontA);
            this.pnlTopBody.Controls.Add(this.btnFontD);
            this.pnlTopBody.Controls.Add(this.btnChgFont);
            this.pnlTopBody.Controls.Add(this.btnSave);
            this.pnlTopBody.Controls.Add(this.txtCn);
            this.pnlTopBody.Controls.Add(this.imgCn);
            this.pnlTopBody.Controls.Add(this.imgJp);
            this.pnlTopBody.Controls.Add(this.pnlLeft);
            this.pnlTopBody.Size = new System.Drawing.Size(746, 216);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.lstPic);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(180, 216);
            this.pnlLeft.TabIndex = 0;
            // 
            // lstPic
            // 
            this.lstPic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstPic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPic.FormattingEnabled = true;
            this.lstPic.ItemHeight = 12;
            this.lstPic.Location = new System.Drawing.Point(0, 0);
            this.lstPic.Name = "lstPic";
            this.lstPic.Size = new System.Drawing.Size(180, 206);
            this.lstPic.TabIndex = 0;
            this.lstPic.SelectedIndexChanged += new System.EventHandler(this.lstMov_SelectedIndexChanged);
            // 
            // imgJp
            // 
            this.imgJp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgJp.Location = new System.Drawing.Point(180, 0);
            this.imgJp.Name = "imgJp";
            this.imgJp.Size = new System.Drawing.Size(458, 70);
            this.imgJp.TabIndex = 1;
            this.imgJp.TabStop = false;
            // 
            // imgCn
            // 
            this.imgCn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgCn.Location = new System.Drawing.Point(180, 75);
            this.imgCn.Name = "imgCn";
            this.imgCn.Size = new System.Drawing.Size(458, 70);
            this.imgCn.TabIndex = 2;
            this.imgCn.TabStop = false;
            // 
            // txtCn
            // 
            this.txtCn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(237)))), ((int)(((byte)(204)))));
            this.txtCn.Font = new System.Drawing.Font("方正超粗黑_GBK", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCn.ForeColor = System.Drawing.Color.White;
            this.txtCn.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtCn.Location = new System.Drawing.Point(180, 151);
            this.txtCn.Multiline = true;
            this.txtCn.Name = "txtCn";
            this.txtCn.Size = new System.Drawing.Size(261, 55);
            this.txtCn.TabIndex = 3;
            this.txtCn.Text = "活下来的人是胜利者！";
            this.txtCn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(539, 179);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 27);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnChgFont
            // 
            this.btnChgFont.Location = new System.Drawing.Point(565, 151);
            this.btnChgFont.Name = "btnChgFont";
            this.btnChgFont.Size = new System.Drawing.Size(61, 27);
            this.btnChgFont.TabIndex = 5;
            this.btnChgFont.Text = "换字库";
            this.btnChgFont.UseVisualStyleBackColor = true;
            this.btnChgFont.Click += new System.EventHandler(this.btnChgFont_Click);
            // 
            // btnFontD
            // 
            this.btnFontD.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFontD.Location = new System.Drawing.Point(539, 151);
            this.btnFontD.Name = "btnFontD";
            this.btnFontD.Size = new System.Drawing.Size(25, 14);
            this.btnFontD.TabIndex = 6;
            this.btnFontD.Text = "-1";
            this.btnFontD.UseVisualStyleBackColor = true;
            this.btnFontD.Click += new System.EventHandler(this.btnFontD_Click);
            // 
            // btnFontA
            // 
            this.btnFontA.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFontA.Location = new System.Drawing.Point(539, 164);
            this.btnFontA.Name = "btnFontA";
            this.btnFontA.Size = new System.Drawing.Size(25, 14);
            this.btnFontA.TabIndex = 7;
            this.btnFontA.Text = "+1";
            this.btnFontA.UseVisualStyleBackColor = true;
            this.btnFontA.Click += new System.EventHandler(this.btnFontA_Click);
            // 
            // txtMoveRight
            // 
            this.txtMoveRight.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtMoveRight.Location = new System.Drawing.Point(463, 170);
            this.txtMoveRight.Name = "txtMoveRight";
            this.txtMoveRight.Size = new System.Drawing.Size(58, 21);
            this.txtMoveRight.TabIndex = 8;
            this.txtMoveRight.Text = "10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(462, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "右移";
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(632, 179);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(87, 27);
            this.btnReload.TabIndex = 10;
            this.btnReload.Text = "刷新";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // ViewtifulJoePicEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 241);
            this.Name = "ViewtifulJoePicEditor";
            this.Text = "红侠乔伊图片文件编辑";
            this.pnlTopBody.ResumeLayout(false);
            this.pnlTopBody.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgJp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.ListBox lstPic;
        private System.Windows.Forms.PictureBox imgJp;
        private System.Windows.Forms.PictureBox imgCn;
        private System.Windows.Forms.TextBox txtCn;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnChgFont;
        private System.Windows.Forms.Button btnFontA;
        private System.Windows.Forms.Button btnFontD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMoveRight;
        private System.Windows.Forms.Button btnReload;
    }
}
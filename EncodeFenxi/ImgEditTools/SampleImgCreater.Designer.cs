namespace Hanhua.ImgEditTools
{
    partial class SampleImgCreater
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
            this.oldImg = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.newImg = new System.Windows.Forms.PictureBox();
            this.txtWenzi = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.btnCreate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.btnChgFont = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.chkBorder = new System.Windows.Forms.CheckBox();
            this.chkColorChg = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.oldImg)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newImg)).BeginInit();
            this.SuspendLayout();
            // 
            // oldImg
            // 
            this.oldImg.BackColor = System.Drawing.SystemColors.Window;
            this.oldImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.oldImg.Location = new System.Drawing.Point(6, 20);
            this.oldImg.Name = "oldImg";
            this.oldImg.Size = new System.Drawing.Size(162, 109);
            this.oldImg.TabIndex = 0;
            this.oldImg.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.oldImg);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(175, 138);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "原始图片";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.newImg);
            this.groupBox2.Location = new System.Drawing.Point(193, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(175, 138);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "新建图片";
            // 
            // newImg
            // 
            this.newImg.BackColor = System.Drawing.SystemColors.Window;
            this.newImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.newImg.Location = new System.Drawing.Point(6, 20);
            this.newImg.Name = "newImg";
            this.newImg.Size = new System.Drawing.Size(162, 109);
            this.newImg.TabIndex = 0;
            this.newImg.TabStop = false;
            // 
            // txtWenzi
            // 
            this.txtWenzi.Font = new System.Drawing.Font("KaiTi", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtWenzi.Location = new System.Drawing.Point(201, 156);
            this.txtWenzi.Name = "txtWenzi";
            this.txtWenzi.Size = new System.Drawing.Size(160, 21);
            this.txtWenzi.TabIndex = 3;
            this.txtWenzi.Text = "小刀＆锁定";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(123, 159);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "输入的文字";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(241, 181);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(57, 23);
            this.btnCreate.TabIndex = 5;
            this.btnCreate.Text = "预览图片";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "X坐标：";
            // 
            // txtX
            // 
            this.txtX.ImeMode = System.Windows.Forms.ImeMode.Close;
            this.txtX.Location = new System.Drawing.Point(64, 156);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(42, 19);
            this.txtX.TabIndex = 6;
            this.txtX.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 186);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "Y坐标：";
            // 
            // txtY
            // 
            this.txtY.ImeMode = System.Windows.Forms.ImeMode.Close;
            this.txtY.Location = new System.Drawing.Point(64, 183);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(42, 19);
            this.txtY.TabIndex = 8;
            this.txtY.Text = "8";
            // 
            // btnChgFont
            // 
            this.btnChgFont.Location = new System.Drawing.Point(178, 181);
            this.btnChgFont.Name = "btnChgFont";
            this.btnChgFont.Size = new System.Drawing.Size(57, 23);
            this.btnChgFont.TabIndex = 10;
            this.btnChgFont.Text = "换字体";
            this.btnChgFont.UseVisualStyleBackColor = true;
            this.btnChgFont.Click += new System.EventHandler(this.btnChgFont_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(304, 181);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(57, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkBorder
            // 
            this.chkBorder.AutoSize = true;
            this.chkBorder.Location = new System.Drawing.Point(13, 210);
            this.chkBorder.Name = "chkBorder";
            this.chkBorder.Size = new System.Drawing.Size(72, 16);
            this.chkBorder.TabIndex = 12;
            this.chkBorder.Text = "有无边框";
            this.chkBorder.UseVisualStyleBackColor = true;
            // 
            // chkColorChg
            // 
            this.chkColorChg.AutoSize = true;
            this.chkColorChg.Location = new System.Drawing.Point(12, 232);
            this.chkColorChg.Name = "chkColorChg";
            this.chkColorChg.Size = new System.Drawing.Size(120, 16);
            this.chkColorChg.TabIndex = 13;
            this.chkColorChg.Text = "背景和字颜色反转";
            this.chkColorChg.UseVisualStyleBackColor = true;
            // 
            // SampleImgCreater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 281);
            this.Controls.Add(this.chkColorChg);
            this.Controls.Add(this.chkBorder);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnChgFont);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtX);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtWenzi);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SampleImgCreater";
            this.Text = "简单图片生成器";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.txtWenzi, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnCreate, 0);
            this.Controls.SetChildIndex(this.txtX, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtY, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.btnChgFont, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.chkBorder, 0);
            this.Controls.SetChildIndex(this.chkColorChg, 0);
            ((System.ComponentModel.ISupportInitialize)(this.oldImg)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.newImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox oldImg;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox newImg;
        private System.Windows.Forms.TextBox txtWenzi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Button btnChgFont;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkBorder;
        private System.Windows.Forms.CheckBox chkColorChg;
    }
}
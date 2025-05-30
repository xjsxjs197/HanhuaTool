namespace Hanhua.Common.TextEditTools.RfoEdit
{
    partial class RfoEdit
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
            this.btnCreateFont = new System.Windows.Forms.Button();
            this.btnCharMap = new System.Windows.Forms.Button();
            this.btnAllTxt = new System.Windows.Forms.Button();
            this.btnSysTxt = new System.Windows.Forms.Button();
            this.btnFontImg = new System.Windows.Forms.Button();
            this.btnSortChar = new System.Windows.Forms.Button();
            this.btnChgPic = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnPatchFile = new System.Windows.Forms.Button();
            this.btnExpSpecialImg = new System.Windows.Forms.Button();
            this.btnImpSpecialImg = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreateFont
            // 
            this.btnCreateFont.Location = new System.Drawing.Point(173, 48);
            this.btnCreateFont.Name = "btnCreateFont";
            this.btnCreateFont.Size = new System.Drawing.Size(143, 30);
            this.btnCreateFont.TabIndex = 0;
            this.btnCreateFont.Text = "字库文件(00530.bin)";
            this.btnCreateFont.UseVisualStyleBackColor = true;
            this.btnCreateFont.Click += new System.EventHandler(this.btnCreateFont_Click);
            // 
            // btnCharMap
            // 
            this.btnCharMap.Location = new System.Drawing.Point(322, 48);
            this.btnCharMap.Name = "btnCharMap";
            this.btnCharMap.Size = new System.Drawing.Size(143, 30);
            this.btnCharMap.TabIndex = 1;
            this.btnCharMap.Text = "字符映射(00529.bin)";
            this.btnCharMap.UseVisualStyleBackColor = true;
            this.btnCharMap.Click += new System.EventHandler(this.btnCharMap_Click);
            // 
            // btnAllTxt
            // 
            this.btnAllTxt.Location = new System.Drawing.Point(28, 84);
            this.btnAllTxt.Name = "btnAllTxt";
            this.btnAllTxt.Size = new System.Drawing.Size(143, 30);
            this.btnAllTxt.TabIndex = 2;
            this.btnAllTxt.Text = "所有文本(00905.bin)";
            this.btnAllTxt.UseVisualStyleBackColor = true;
            this.btnAllTxt.Click += new System.EventHandler(this.btnAllTxt_Click);
            // 
            // btnSysTxt
            // 
            this.btnSysTxt.Location = new System.Drawing.Point(173, 84);
            this.btnSysTxt.Name = "btnSysTxt";
            this.btnSysTxt.Size = new System.Drawing.Size(143, 30);
            this.btnSysTxt.TabIndex = 3;
            this.btnSysTxt.Text = "系统文本(01718.bin)";
            this.btnSysTxt.UseVisualStyleBackColor = true;
            this.btnSysTxt.Click += new System.EventHandler(this.btnSysTxt_Click);
            // 
            // btnFontImg
            // 
            this.btnFontImg.Location = new System.Drawing.Point(28, 48);
            this.btnFontImg.Name = "btnFontImg";
            this.btnFontImg.Size = new System.Drawing.Size(143, 30);
            this.btnFontImg.TabIndex = 4;
            this.btnFontImg.Text = "字库图片做成";
            this.btnFontImg.UseVisualStyleBackColor = true;
            this.btnFontImg.Click += new System.EventHandler(this.btnFontImg_Click);
            // 
            // btnSortChar
            // 
            this.btnSortChar.Location = new System.Drawing.Point(28, 12);
            this.btnSortChar.Name = "btnSortChar";
            this.btnSortChar.Size = new System.Drawing.Size(143, 30);
            this.btnSortChar.TabIndex = 5;
            this.btnSortChar.Text = "字库文字排序";
            this.btnSortChar.UseVisualStyleBackColor = true;
            this.btnSortChar.Click += new System.EventHandler(this.btnSortChar_Click);
            // 
            // btnChgPic
            // 
            this.btnChgPic.Location = new System.Drawing.Point(28, 120);
            this.btnChgPic.Name = "btnChgPic";
            this.btnChgPic.Size = new System.Drawing.Size(143, 30);
            this.btnChgPic.TabIndex = 6;
            this.btnChgPic.Text = "另存汉化图片";
            this.btnChgPic.UseVisualStyleBackColor = true;
            this.btnChgPic.Click += new System.EventHandler(this.btnChgPic_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(322, 84);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(143, 30);
            this.btnTest.TabIndex = 7;
            this.btnTest.Text = "测试按钮";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnPatchFile
            // 
            this.btnPatchFile.Location = new System.Drawing.Point(28, 156);
            this.btnPatchFile.Name = "btnPatchFile";
            this.btnPatchFile.Size = new System.Drawing.Size(143, 30);
            this.btnPatchFile.TabIndex = 8;
            this.btnPatchFile.Text = "一键打包";
            this.btnPatchFile.UseVisualStyleBackColor = true;
            this.btnPatchFile.Click += new System.EventHandler(this.btnPatchFile_Click);
            // 
            // btnExpSpecialImg
            // 
            this.btnExpSpecialImg.Location = new System.Drawing.Point(173, 120);
            this.btnExpSpecialImg.Name = "btnExpSpecialImg";
            this.btnExpSpecialImg.Size = new System.Drawing.Size(143, 30);
            this.btnExpSpecialImg.TabIndex = 9;
            this.btnExpSpecialImg.Text = "导出特殊图片";
            this.btnExpSpecialImg.UseVisualStyleBackColor = true;
            this.btnExpSpecialImg.Click += new System.EventHandler(this.btnExpSpecialImg_Click);
            // 
            // btnImpSpecialImg
            // 
            this.btnImpSpecialImg.Location = new System.Drawing.Point(322, 120);
            this.btnImpSpecialImg.Name = "btnImpSpecialImg";
            this.btnImpSpecialImg.Size = new System.Drawing.Size(143, 30);
            this.btnImpSpecialImg.TabIndex = 10;
            this.btnImpSpecialImg.Text = "导入特殊图片";
            this.btnImpSpecialImg.UseVisualStyleBackColor = true;
            this.btnImpSpecialImg.Click += new System.EventHandler(this.btnImpSpecialImg_Click);
            // 
            // RfoEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 200);
            this.Controls.Add(this.btnImpSpecialImg);
            this.Controls.Add(this.btnExpSpecialImg);
            this.Controls.Add(this.btnPatchFile);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnChgPic);
            this.Controls.Add(this.btnSortChar);
            this.Controls.Add(this.btnFontImg);
            this.Controls.Add(this.btnSysTxt);
            this.Controls.Add(this.btnAllTxt);
            this.Controls.Add(this.btnCharMap);
            this.Controls.Add(this.btnCreateFont);
            this.MaximizeBox = false;
            this.Name = "RfoEdit";
            this.Text = "符文工房边境";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreateFont;
        private System.Windows.Forms.Button btnCharMap;
        private System.Windows.Forms.Button btnAllTxt;
        private System.Windows.Forms.Button btnSysTxt;
        private System.Windows.Forms.Button btnFontImg;
        private System.Windows.Forms.Button btnSortChar;
        private System.Windows.Forms.Button btnChgPic;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnPatchFile;
        private System.Windows.Forms.Button btnExpSpecialImg;
        private System.Windows.Forms.Button btnImpSpecialImg;
    }
}
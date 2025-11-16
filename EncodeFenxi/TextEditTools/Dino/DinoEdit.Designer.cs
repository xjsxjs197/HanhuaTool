namespace Hanhua.Common.TextEditTools.Dino
{
    partial class DinoEdit
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
            this.btnViewDat = new System.Windows.Forms.Button();
            this.btnComDat = new System.Windows.Forms.Button();
            this.txtChk = new System.Windows.Forms.TextBox();
            this.btnSearchTxt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnViewDat
            // 
            this.btnViewDat.Location = new System.Drawing.Point(12, 21);
            this.btnViewDat.Name = "btnViewDat";
            this.btnViewDat.Size = new System.Drawing.Size(129, 31);
            this.btnViewDat.TabIndex = 0;
            this.btnViewDat.Text = ".DAT查看";
            this.btnViewDat.UseVisualStyleBackColor = true;
            this.btnViewDat.Click += new System.EventHandler(this.btnViewDat_Click);
            // 
            // btnComDat
            // 
            this.btnComDat.Location = new System.Drawing.Point(159, 21);
            this.btnComDat.Name = "btnComDat";
            this.btnComDat.Size = new System.Drawing.Size(129, 31);
            this.btnComDat.TabIndex = 1;
            this.btnComDat.Text = ".DAT压缩";
            this.btnComDat.UseVisualStyleBackColor = true;
            this.btnComDat.Click += new System.EventHandler(this.btnComDat_Click);
            // 
            // txtChk
            // 
            this.txtChk.Location = new System.Drawing.Point(12, 76);
            this.txtChk.Name = "txtChk";
            this.txtChk.Size = new System.Drawing.Size(180, 21);
            this.txtChk.TabIndex = 2;
            // 
            // btnSearchTxt
            // 
            this.btnSearchTxt.Location = new System.Drawing.Point(208, 71);
            this.btnSearchTxt.Name = "btnSearchTxt";
            this.btnSearchTxt.Size = new System.Drawing.Size(132, 28);
            this.btnSearchTxt.TabIndex = 3;
            this.btnSearchTxt.Text = "查看文本位置";
            this.btnSearchTxt.UseVisualStyleBackColor = true;
            this.btnSearchTxt.Click += new System.EventHandler(this.btnSearchTxt_Click);
            // 
            // DinoEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 137);
            this.Controls.Add(this.btnSearchTxt);
            this.Controls.Add(this.txtChk);
            this.Controls.Add(this.btnComDat);
            this.Controls.Add(this.btnViewDat);
            this.Name = "DinoEdit";
            this.Text = "DinoEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnViewDat;
        private System.Windows.Forms.Button btnComDat;
        private System.Windows.Forms.TextBox txtChk;
        private System.Windows.Forms.Button btnSearchTxt;
    }
}
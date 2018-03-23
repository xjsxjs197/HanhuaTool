namespace Hanhua.Common.Bio0Edit
{
    partial class Bio0LzTool
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
            this.btnDec = new System.Windows.Forms.Button();
            this.btnCom = new System.Windows.Forms.Button();
            this.btnCreateMes = new System.Windows.Forms.Button();
            this.btnAllDec = new System.Windows.Forms.Button();
            this.btnAllCom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDec
            // 
            this.btnDec.Location = new System.Drawing.Point(12, 10);
            this.btnDec.Name = "btnDec";
            this.btnDec.Size = new System.Drawing.Size(91, 23);
            this.btnDec.TabIndex = 0;
            this.btnDec.Text = "解压缩";
            this.btnDec.UseVisualStyleBackColor = true;
            this.btnDec.Click += new System.EventHandler(this.btnDec_Click);
            // 
            // btnCom
            // 
            this.btnCom.Location = new System.Drawing.Point(127, 10);
            this.btnCom.Name = "btnCom";
            this.btnCom.Size = new System.Drawing.Size(91, 23);
            this.btnCom.TabIndex = 1;
            this.btnCom.Text = "压缩";
            this.btnCom.UseVisualStyleBackColor = true;
            this.btnCom.Click += new System.EventHandler(this.btnCom_Click);
            // 
            // btnCreateMes
            // 
            this.btnCreateMes.Location = new System.Drawing.Point(12, 67);
            this.btnCreateMes.Name = "btnCreateMes";
            this.btnCreateMes.Size = new System.Drawing.Size(91, 23);
            this.btnCreateMes.TabIndex = 2;
            this.btnCreateMes.Text = "生成Message";
            this.btnCreateMes.UseVisualStyleBackColor = true;
            this.btnCreateMes.Click += new System.EventHandler(this.btnCreateMes_Click);
            // 
            // btnAllDec
            // 
            this.btnAllDec.Location = new System.Drawing.Point(12, 39);
            this.btnAllDec.Name = "btnAllDec";
            this.btnAllDec.Size = new System.Drawing.Size(91, 23);
            this.btnAllDec.TabIndex = 3;
            this.btnAllDec.Text = "批量解压缩";
            this.btnAllDec.UseVisualStyleBackColor = true;
            this.btnAllDec.Click += new System.EventHandler(this.btnAllDec_Click);
            // 
            // btnAllCom
            // 
            this.btnAllCom.Location = new System.Drawing.Point(127, 39);
            this.btnAllCom.Name = "btnAllCom";
            this.btnAllCom.Size = new System.Drawing.Size(91, 23);
            this.btnAllCom.TabIndex = 4;
            this.btnAllCom.Text = "批量压缩";
            this.btnAllCom.UseVisualStyleBackColor = true;
            this.btnAllCom.Click += new System.EventHandler(this.btnAllCom_Click);
            // 
            // Bio0LzTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 122);
            this.Controls.Add(this.btnAllCom);
            this.Controls.Add(this.btnAllDec);
            this.Controls.Add(this.btnCreateMes);
            this.Controls.Add(this.btnCom);
            this.Controls.Add(this.btnDec);
            this.Name = "Bio0LzTool";
            this.Text = "Bio0LzTool";
            this.Controls.SetChildIndex(this.btnDec, 0);
            this.Controls.SetChildIndex(this.btnCom, 0);
            this.Controls.SetChildIndex(this.btnCreateMes, 0);
            this.Controls.SetChildIndex(this.btnAllDec, 0);
            this.Controls.SetChildIndex(this.btnAllCom, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDec;
        private System.Windows.Forms.Button btnCom;
        private System.Windows.Forms.Button btnCreateMes;
        private System.Windows.Forms.Button btnAllDec;
        private System.Windows.Forms.Button btnAllCom;
    }
}
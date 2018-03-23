namespace Hanhua.Common.Bio3Edit
{
    partial class Bio3CpoyTextFromPs
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtJpFolder = new System.Windows.Forms.TextBox();
            this.btnJpSelect = new System.Windows.Forms.Button();
            this.btnCnSelect = new System.Windows.Forms.Button();
            this.txtCnFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnNgcSelect = new System.Windows.Forms.Button();
            this.txtNgcFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ps日文版目录：";
            // 
            // txtJpFolder
            // 
            this.txtJpFolder.Location = new System.Drawing.Point(113, 14);
            this.txtJpFolder.Name = "txtJpFolder";
            this.txtJpFolder.ReadOnly = true;
            this.txtJpFolder.Size = new System.Drawing.Size(253, 19);
            this.txtJpFolder.TabIndex = 1;
            // 
            // btnJpSelect
            // 
            this.btnJpSelect.Location = new System.Drawing.Point(381, 12);
            this.btnJpSelect.Name = "btnJpSelect";
            this.btnJpSelect.Size = new System.Drawing.Size(87, 21);
            this.btnJpSelect.TabIndex = 2;
            this.btnJpSelect.Text = "选择目录";
            this.btnJpSelect.UseVisualStyleBackColor = true;
            // 
            // btnCnSelect
            // 
            this.btnCnSelect.Location = new System.Drawing.Point(381, 36);
            this.btnCnSelect.Name = "btnCnSelect";
            this.btnCnSelect.Size = new System.Drawing.Size(87, 21);
            this.btnCnSelect.TabIndex = 5;
            this.btnCnSelect.Text = "选择目录";
            this.btnCnSelect.UseVisualStyleBackColor = true;
            // 
            // txtCnFolder
            // 
            this.txtCnFolder.Location = new System.Drawing.Point(113, 38);
            this.txtCnFolder.Name = "txtCnFolder";
            this.txtCnFolder.ReadOnly = true;
            this.txtCnFolder.Size = new System.Drawing.Size(253, 19);
            this.txtCnFolder.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ps中文版目录：";
            // 
            // btnNgcSelect
            // 
            this.btnNgcSelect.Location = new System.Drawing.Point(381, 69);
            this.btnNgcSelect.Name = "btnNgcSelect";
            this.btnNgcSelect.Size = new System.Drawing.Size(87, 21);
            this.btnNgcSelect.TabIndex = 8;
            this.btnNgcSelect.Text = "选择目录";
            this.btnNgcSelect.UseVisualStyleBackColor = true;
            // 
            // txtNgcFolder
            // 
            this.txtNgcFolder.Location = new System.Drawing.Point(113, 71);
            this.txtNgcFolder.Name = "txtNgcFolder";
            this.txtNgcFolder.ReadOnly = true;
            this.txtNgcFolder.Size = new System.Drawing.Size(253, 19);
            this.txtNgcFolder.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Ngc版目录：";
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(174, 95);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(113, 21);
            this.btnCopy.TabIndex = 9;
            this.btnCopy.Text = "开始复制Ps版文本";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // Bio3CpoyTextFromPs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 148);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnNgcSelect);
            this.Controls.Add(this.txtNgcFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCnSelect);
            this.Controls.Add(this.txtCnFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnJpSelect);
            this.Controls.Add(this.txtJpFolder);
            this.Controls.Add(this.label1);
            this.Name = "Bio3CpoyTextFromPs";
            this.Text = "从Ps版Copy文本";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtJpFolder, 0);
            this.Controls.SetChildIndex(this.btnJpSelect, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtCnFolder, 0);
            this.Controls.SetChildIndex(this.btnCnSelect, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtNgcFolder, 0);
            this.Controls.SetChildIndex(this.btnNgcSelect, 0);
            this.Controls.SetChildIndex(this.btnCopy, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtJpFolder;
        private System.Windows.Forms.Button btnJpSelect;
        private System.Windows.Forms.Button btnCnSelect;
        private System.Windows.Forms.TextBox txtCnFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnNgcSelect;
        private System.Windows.Forms.TextBox txtNgcFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCopy;
    }
}
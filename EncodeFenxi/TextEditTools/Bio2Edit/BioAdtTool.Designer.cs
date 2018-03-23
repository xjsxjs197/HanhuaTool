namespace Hanhua.TextEditTools.BioAdt
{
    partial class BioAdtTool
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
            this.btnAdtDec = new System.Windows.Forms.Button();
            this.btnTimSelect = new System.Windows.Forms.Button();
            this.txtTimFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAdtSelect = new System.Windows.Forms.Button();
            this.txtAdtFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rdoDec = new System.Windows.Forms.RadioButton();
            this.rdoCmp = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnAdtDec
            // 
            this.btnAdtDec.Location = new System.Drawing.Point(208, 65);
            this.btnAdtDec.Name = "btnAdtDec";
            this.btnAdtDec.Size = new System.Drawing.Size(113, 21);
            this.btnAdtDec.TabIndex = 16;
            this.btnAdtDec.Text = "开始处理";
            this.btnAdtDec.UseVisualStyleBackColor = true;
            this.btnAdtDec.Click += new System.EventHandler(this.btnAdtDec_Click);
            // 
            // btnTimSelect
            // 
            this.btnTimSelect.Location = new System.Drawing.Point(371, 28);
            this.btnTimSelect.Name = "btnTimSelect";
            this.btnTimSelect.Size = new System.Drawing.Size(87, 21);
            this.btnTimSelect.TabIndex = 15;
            this.btnTimSelect.Text = "选择目录";
            this.btnTimSelect.UseVisualStyleBackColor = true;
            this.btnTimSelect.Click += new System.EventHandler(this.btnCnSelect_Click);
            // 
            // txtTimFolder
            // 
            this.txtTimFolder.Location = new System.Drawing.Point(103, 30);
            this.txtTimFolder.Name = "txtTimFolder";
            this.txtTimFolder.ReadOnly = true;
            this.txtTimFolder.Size = new System.Drawing.Size(253, 19);
            this.txtTimFolder.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "Tim文件目录：";
            // 
            // btnAdtSelect
            // 
            this.btnAdtSelect.Location = new System.Drawing.Point(371, 4);
            this.btnAdtSelect.Name = "btnAdtSelect";
            this.btnAdtSelect.Size = new System.Drawing.Size(87, 21);
            this.btnAdtSelect.TabIndex = 12;
            this.btnAdtSelect.Text = "选择目录";
            this.btnAdtSelect.UseVisualStyleBackColor = true;
            this.btnAdtSelect.Click += new System.EventHandler(this.btnJpSelect_Click);
            // 
            // txtAdtFolder
            // 
            this.txtAdtFolder.Location = new System.Drawing.Point(103, 6);
            this.txtAdtFolder.Name = "txtAdtFolder";
            this.txtAdtFolder.ReadOnly = true;
            this.txtAdtFolder.Size = new System.Drawing.Size(253, 19);
            this.txtAdtFolder.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "Adt文件目录：";
            // 
            // rdoDec
            // 
            this.rdoDec.AutoSize = true;
            this.rdoDec.Checked = true;
            this.rdoDec.Location = new System.Drawing.Point(15, 65);
            this.rdoDec.Name = "rdoDec";
            this.rdoDec.Size = new System.Drawing.Size(59, 16);
            this.rdoDec.TabIndex = 17;
            this.rdoDec.TabStop = true;
            this.rdoDec.Text = "解压缩";
            this.rdoDec.UseVisualStyleBackColor = true;
            // 
            // rdoCmp
            // 
            this.rdoCmp.AutoSize = true;
            this.rdoCmp.Location = new System.Drawing.Point(82, 65);
            this.rdoCmp.Name = "rdoCmp";
            this.rdoCmp.Size = new System.Drawing.Size(47, 16);
            this.rdoCmp.TabIndex = 18;
            this.rdoCmp.Text = "压缩";
            this.rdoCmp.UseVisualStyleBackColor = true;
            // 
            // BioAdtTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 118);
            this.Controls.Add(this.rdoCmp);
            this.Controls.Add(this.rdoDec);
            this.Controls.Add(this.btnAdtDec);
            this.Controls.Add(this.btnTimSelect);
            this.Controls.Add(this.txtTimFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAdtSelect);
            this.Controls.Add(this.txtAdtFolder);
            this.Controls.Add(this.label1);
            this.Name = "BioAdtTool";
            this.Text = "BioAdtTool";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtAdtFolder, 0);
            this.Controls.SetChildIndex(this.btnAdtSelect, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtTimFolder, 0);
            this.Controls.SetChildIndex(this.btnTimSelect, 0);
            this.Controls.SetChildIndex(this.btnAdtDec, 0);
            this.Controls.SetChildIndex(this.rdoDec, 0);
            this.Controls.SetChildIndex(this.rdoCmp, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAdtDec;
        private System.Windows.Forms.Button btnTimSelect;
        private System.Windows.Forms.TextBox txtTimFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAdtSelect;
        private System.Windows.Forms.TextBox txtAdtFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdoDec;
        private System.Windows.Forms.RadioButton rdoCmp;
    }
}
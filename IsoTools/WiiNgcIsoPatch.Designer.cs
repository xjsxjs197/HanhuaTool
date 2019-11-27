namespace IsoTools
{
    partial class WiiNgcIsoPatch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WiiNgcIsoPatch));
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnCnSelect = new System.Windows.Forms.Button();
            this.txtCnFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnJpSelect = new System.Windows.Forms.Button();
            this.txtJpIso = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSameSize = new System.Windows.Forms.CheckBox();
            this.chkNoDec = new System.Windows.Forms.CheckBox();
            this.chkSaveTemp = new System.Windows.Forms.CheckBox();
            this.pnlTopBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Controls.Add(this.chkSaveTemp);
            this.pnlTopBody.Controls.Add(this.chkNoDec);
            this.pnlTopBody.Size = new System.Drawing.Size(504, 191);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(181, 148);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(113, 31);
            this.btnCopy.TabIndex = 16;
            this.btnCopy.Text = "打  补  丁";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnCnSelect
            // 
            this.btnCnSelect.Location = new System.Drawing.Point(388, 114);
            this.btnCnSelect.Name = "btnCnSelect";
            this.btnCnSelect.Size = new System.Drawing.Size(87, 21);
            this.btnCnSelect.TabIndex = 2;
            this.btnCnSelect.Text = "选择目录";
            this.btnCnSelect.UseVisualStyleBackColor = true;
            this.btnCnSelect.Click += new System.EventHandler(this.btnCnSelect_Click);
            // 
            // txtCnFolder
            // 
            this.txtCnFolder.Location = new System.Drawing.Point(120, 116);
            this.txtCnFolder.Name = "txtCnFolder";
            this.txtCnFolder.ReadOnly = true;
            this.txtCnFolder.Size = new System.Drawing.Size(253, 19);
            this.txtCnFolder.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "中文补丁目录：";
            // 
            // btnJpSelect
            // 
            this.btnJpSelect.Location = new System.Drawing.Point(388, 21);
            this.btnJpSelect.Name = "btnJpSelect";
            this.btnJpSelect.Size = new System.Drawing.Size(87, 21);
            this.btnJpSelect.TabIndex = 3;
            this.btnJpSelect.Text = "选择文件";
            this.btnJpSelect.UseVisualStyleBackColor = true;
            this.btnJpSelect.Click += new System.EventHandler(this.btnJpSelect_Click);
            // 
            // txtJpIso
            // 
            this.txtJpIso.Location = new System.Drawing.Point(120, 23);
            this.txtJpIso.Name = "txtJpIso";
            this.txtJpIso.ReadOnly = true;
            this.txtJpIso.Size = new System.Drawing.Size(253, 19);
            this.txtJpIso.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "日文Iso文件：";
            // 
            // chkSameSize
            // 
            this.chkSameSize.AutoSize = true;
            this.chkSameSize.Location = new System.Drawing.Point(126, 46);
            this.chkSameSize.Name = "chkSameSize";
            this.chkSameSize.Size = new System.Drawing.Size(238, 16);
            this.chkSameSize.TabIndex = 17;
            this.chkSameSize.Text = "保持内部文件大小、位置一致（Ngc生化0）";
            this.chkSameSize.UseVisualStyleBackColor = true;
            // 
            // chkNoDec
            // 
            this.chkNoDec.AutoSize = true;
            this.chkNoDec.Location = new System.Drawing.Point(126, 68);
            this.chkNoDec.Name = "chkNoDec";
            this.chkNoDec.Size = new System.Drawing.Size(147, 16);
            this.chkNoDec.TabIndex = 18;
            this.chkNoDec.Text = "已存在加Wii解压缩文件";
            this.chkNoDec.UseVisualStyleBackColor = true;
            // 
            // chkSaveTemp
            // 
            this.chkSaveTemp.AutoSize = true;
            this.chkSaveTemp.Location = new System.Drawing.Point(126, 90);
            this.chkSaveTemp.Name = "chkSaveTemp";
            this.chkSaveTemp.Size = new System.Drawing.Size(120, 16);
            this.chkSaveTemp.TabIndex = 19;
            this.chkSaveTemp.Text = "不要删除临时文件";
            this.chkSaveTemp.UseVisualStyleBackColor = true;
            // 
            // WiiNgcIsoPatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 216);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnCnSelect);
            this.Controls.Add(this.chkSameSize);
            this.Controls.Add(this.txtCnFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnJpSelect);
            this.Controls.Add(this.txtJpIso);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WiiNgcIsoPatch";
            this.Text = "Wii/Ngc Iso打补丁工具(2017/01/19)";
            this.Controls.SetChildIndex(this.pnlTopBody, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtJpIso, 0);
            this.Controls.SetChildIndex(this.btnJpSelect, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtCnFolder, 0);
            this.Controls.SetChildIndex(this.chkSameSize, 0);
            this.Controls.SetChildIndex(this.btnCnSelect, 0);
            this.Controls.SetChildIndex(this.btnCopy, 0);
            this.pnlTopBody.ResumeLayout(false);
            this.pnlTopBody.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnCnSelect;
        private System.Windows.Forms.TextBox txtCnFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnJpSelect;
        private System.Windows.Forms.TextBox txtJpIso;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSameSize;
        private System.Windows.Forms.CheckBox chkNoDec;
        private System.Windows.Forms.CheckBox chkSaveTemp;
    }
}
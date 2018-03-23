using Hanhua.Common;
namespace Hanhua.TextEditTools.Bio2Edit
{
    partial class Bio2TextEditor
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
            this.btnPatch = new System.Windows.Forms.Button();
            this.rdoNgc = new System.Windows.Forms.RadioButton();
            this.rdoPs = new System.Windows.Forms.RadioButton();
            this.chkNgcDol = new System.Windows.Forms.CheckBox();
            this.chkPsBin = new System.Windows.Forms.CheckBox();
            this.btnReLoad = new System.Windows.Forms.Button();
            this.btnCopyFromPs = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoBDisk = new System.Windows.Forms.RadioButton();
            this.rdoADisk = new System.Windows.Forms.RadioButton();
            this.chkNgcRdt = new System.Windows.Forms.CheckBox();
            this.pnlCommand.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCommand
            // 
            this.pnlCommand.Controls.Add(this.chkNgcRdt);
            this.pnlCommand.Controls.Add(this.btnCopyFromPs);
            this.pnlCommand.Controls.Add(this.btnReLoad);
            this.pnlCommand.Controls.Add(this.chkPsBin);
            this.pnlCommand.Controls.Add(this.panel1);
            this.pnlCommand.Controls.Add(this.chkNgcDol);
            this.pnlCommand.Controls.Add(this.rdoPs);
            this.pnlCommand.Controls.Add(this.rdoNgc);
            this.pnlCommand.Controls.Add(this.btnPatch);
            this.pnlCommand.Location = new System.Drawing.Point(0, 550);
            this.pnlCommand.Controls.SetChildIndex(this.btnPatch, 0);
            this.pnlCommand.Controls.SetChildIndex(this.rdoNgc, 0);
            this.pnlCommand.Controls.SetChildIndex(this.rdoPs, 0);
            this.pnlCommand.Controls.SetChildIndex(this.chkNgcDol, 0);
            this.pnlCommand.Controls.SetChildIndex(this.panel1, 0);
            this.pnlCommand.Controls.SetChildIndex(this.chkPsBin, 0);
            this.pnlCommand.Controls.SetChildIndex(this.btnReLoad, 0);
            this.pnlCommand.Controls.SetChildIndex(this.btnCopyFromPs, 0);
            this.pnlCommand.Controls.SetChildIndex(this.chkNgcRdt, 0);
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Size = new System.Drawing.Size(912, 584);
            // 
            // btnPatch
            // 
            this.btnPatch.Location = new System.Drawing.Point(493, 4);
            this.btnPatch.Name = "btnPatch";
            this.btnPatch.Size = new System.Drawing.Size(60, 26);
            this.btnPatch.TabIndex = 6;
            this.btnPatch.Text = "打包";
            this.btnPatch.UseVisualStyleBackColor = true;
            this.btnPatch.Click += new System.EventHandler(this.btnPatch_Click);
            // 
            // rdoNgc
            // 
            this.rdoNgc.AutoSize = true;
            this.rdoNgc.Checked = true;
            this.rdoNgc.Location = new System.Drawing.Point(25, 9);
            this.rdoNgc.Name = "rdoNgc";
            this.rdoNgc.Size = new System.Drawing.Size(41, 16);
            this.rdoNgc.TabIndex = 7;
            this.rdoNgc.TabStop = true;
            this.rdoNgc.Text = "Ngc";
            this.rdoNgc.UseVisualStyleBackColor = true;
            this.rdoNgc.CheckedChanged += new System.EventHandler(this.rdoNgc_CheckedChanged);
            // 
            // rdoPs
            // 
            this.rdoPs.AutoSize = true;
            this.rdoPs.Location = new System.Drawing.Point(202, 10);
            this.rdoPs.Name = "rdoPs";
            this.rdoPs.Size = new System.Drawing.Size(35, 16);
            this.rdoPs.TabIndex = 8;
            this.rdoPs.Text = "Ps";
            this.rdoPs.UseVisualStyleBackColor = true;
            // 
            // chkNgcDol
            // 
            this.chkNgcDol.AutoSize = true;
            this.chkNgcDol.Location = new System.Drawing.Point(74, 10);
            this.chkNgcDol.Name = "chkNgcDol";
            this.chkNgcDol.Size = new System.Drawing.Size(78, 16);
            this.chkNgcDol.TabIndex = 9;
            this.chkNgcDol.Text = "Start.dol";
            this.chkNgcDol.UseVisualStyleBackColor = true;
            // 
            // chkPsBin
            // 
            this.chkPsBin.AutoSize = true;
            this.chkPsBin.Location = new System.Drawing.Point(244, 10);
            this.chkPsBin.Name = "chkPsBin";
            this.chkPsBin.Size = new System.Drawing.Size(42, 16);
            this.chkPsBin.TabIndex = 12;
            this.chkPsBin.Text = "Bin";
            this.chkPsBin.UseVisualStyleBackColor = true;
            // 
            // btnReLoad
            // 
            this.btnReLoad.Location = new System.Drawing.Point(427, 4);
            this.btnReLoad.Name = "btnReLoad";
            this.btnReLoad.Size = new System.Drawing.Size(60, 26);
            this.btnReLoad.TabIndex = 14;
            this.btnReLoad.Text = "刷新";
            this.btnReLoad.UseVisualStyleBackColor = true;
            this.btnReLoad.Click += new System.EventHandler(this.btnReLoad_Click);
            // 
            // btnCopyFromPs
            // 
            this.btnCopyFromPs.Location = new System.Drawing.Point(559, 4);
            this.btnCopyFromPs.Name = "btnCopyFromPs";
            this.btnCopyFromPs.Size = new System.Drawing.Size(60, 26);
            this.btnCopyFromPs.TabIndex = 15;
            this.btnCopyFromPs.Text = "Ps->Ngc";
            this.btnCopyFromPs.UseVisualStyleBackColor = true;
            this.btnCopyFromPs.Click += new System.EventHandler(this.btnCopyFromPs_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.rdoBDisk);
            this.panel1.Controls.Add(this.rdoADisk);
            this.panel1.Location = new System.Drawing.Point(329, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(60, 34);
            this.panel1.TabIndex = 18;
            // 
            // rdoBDisk
            // 
            this.rdoBDisk.AutoSize = true;
            this.rdoBDisk.Location = new System.Drawing.Point(9, 17);
            this.rdoBDisk.Name = "rdoBDisk";
            this.rdoBDisk.Size = new System.Drawing.Size(41, 16);
            this.rdoBDisk.TabIndex = 9;
            this.rdoBDisk.Text = "B盘";
            this.rdoBDisk.UseVisualStyleBackColor = true;
            // 
            // rdoADisk
            // 
            this.rdoADisk.AutoSize = true;
            this.rdoADisk.Checked = true;
            this.rdoADisk.Location = new System.Drawing.Point(9, 1);
            this.rdoADisk.Name = "rdoADisk";
            this.rdoADisk.Size = new System.Drawing.Size(41, 16);
            this.rdoADisk.TabIndex = 8;
            this.rdoADisk.TabStop = true;
            this.rdoADisk.Text = "A盘";
            this.rdoADisk.UseVisualStyleBackColor = true;
            this.rdoADisk.CheckedChanged += new System.EventHandler(this.rdoADisk_CheckedChanged);
            // 
            // chkNgcRdt
            // 
            this.chkNgcRdt.AutoSize = true;
            this.chkNgcRdt.Checked = true;
            this.chkNgcRdt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNgcRdt.Location = new System.Drawing.Point(154, 10);
            this.chkNgcRdt.Name = "chkNgcRdt";
            this.chkNgcRdt.Size = new System.Drawing.Size(42, 16);
            this.chkNgcRdt.TabIndex = 19;
            this.chkNgcRdt.Text = "Rdt";
            this.chkNgcRdt.UseVisualStyleBackColor = true;
            // 
            // Bio2TextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 609);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "Bio2TextEditor";
            this.Text = "文本查看";
            this.pnlCommand.ResumeLayout(false);
            this.pnlCommand.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPatch;
        private System.Windows.Forms.RadioButton rdoNgc;
        private System.Windows.Forms.RadioButton rdoPs;
        private System.Windows.Forms.CheckBox chkNgcDol;
        private System.Windows.Forms.CheckBox chkPsBin;
        private System.Windows.Forms.Button btnReLoad;
        private System.Windows.Forms.Button btnCopyFromPs;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdoBDisk;
        private System.Windows.Forms.RadioButton rdoADisk;
        private System.Windows.Forms.CheckBox chkNgcRdt;

    }
}
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
            this.rdoNgc = new System.Windows.Forms.RadioButton();
            this.rdoPs = new System.Windows.Forms.RadioButton();
            this.chkNgcDol = new System.Windows.Forms.CheckBox();
            this.chkPsBin = new System.Windows.Forms.CheckBox();
            this.btnCopyFromPs = new System.Windows.Forms.Button();
            this.chkNgcRdt = new System.Windows.Forms.CheckBox();
            this.pnlCommand.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCommand
            // 
            this.pnlCommand.Controls.Add(this.chkNgcRdt);
            this.pnlCommand.Controls.Add(this.btnCopyFromPs);
            this.pnlCommand.Controls.Add(this.chkPsBin);
            this.pnlCommand.Controls.Add(this.chkNgcDol);
            this.pnlCommand.Controls.Add(this.rdoPs);
            this.pnlCommand.Controls.Add(this.rdoNgc);
            this.pnlCommand.Location = new System.Drawing.Point(0, 550);
            this.pnlCommand.Controls.SetChildIndex(this.rdoNgc, 0);
            this.pnlCommand.Controls.SetChildIndex(this.rdoPs, 0);
            this.pnlCommand.Controls.SetChildIndex(this.chkNgcDol, 0);
            this.pnlCommand.Controls.SetChildIndex(this.chkPsBin, 0);
            this.pnlCommand.Controls.SetChildIndex(this.btnCopyFromPs, 0);
            this.pnlCommand.Controls.SetChildIndex(this.chkNgcRdt, 0);
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Size = new System.Drawing.Size(912, 584);
            // 
            // rdoNgc
            // 
            this.rdoNgc.AutoSize = true;
            this.rdoNgc.Checked = true;
            this.rdoNgc.Location = new System.Drawing.Point(25, 9);
            this.rdoNgc.Name = "rdoNgc";
            this.rdoNgc.Size = new System.Drawing.Size(43, 16);
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
            this.rdoPs.Size = new System.Drawing.Size(36, 16);
            this.rdoPs.TabIndex = 8;
            this.rdoPs.Text = "Ps";
            this.rdoPs.UseVisualStyleBackColor = true;
            // 
            // chkNgcDol
            // 
            this.chkNgcDol.AutoSize = true;
            this.chkNgcDol.Location = new System.Drawing.Point(74, 10);
            this.chkNgcDol.Name = "chkNgcDol";
            this.chkNgcDol.Size = new System.Drawing.Size(66, 16);
            this.chkNgcDol.TabIndex = 9;
            this.chkNgcDol.Text = "Start.dol";
            this.chkNgcDol.UseVisualStyleBackColor = true;
            // 
            // chkPsBin
            // 
            this.chkPsBin.AutoSize = true;
            this.chkPsBin.Location = new System.Drawing.Point(244, 10);
            this.chkPsBin.Name = "chkPsBin";
            this.chkPsBin.Size = new System.Drawing.Size(41, 16);
            this.chkPsBin.TabIndex = 12;
            this.chkPsBin.Text = "Bin";
            this.chkPsBin.UseVisualStyleBackColor = true;
            // 
            // btnCopyFromPs
            // 
            this.btnCopyFromPs.Location = new System.Drawing.Point(335, 4);
            this.btnCopyFromPs.Name = "btnCopyFromPs";
            this.btnCopyFromPs.Size = new System.Drawing.Size(60, 26);
            this.btnCopyFromPs.TabIndex = 15;
            this.btnCopyFromPs.Text = "Ps->Ngc";
            this.btnCopyFromPs.UseVisualStyleBackColor = true;
            this.btnCopyFromPs.Click += new System.EventHandler(this.btnCopyFromPs_Click);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoNgc;
        private System.Windows.Forms.RadioButton rdoPs;
        private System.Windows.Forms.CheckBox chkNgcDol;
        private System.Windows.Forms.CheckBox chkPsBin;
        private System.Windows.Forms.Button btnCopyFromPs;
        private System.Windows.Forms.CheckBox chkNgcRdt;

    }
}
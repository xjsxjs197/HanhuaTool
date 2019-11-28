using Hanhua.Common;
namespace Hanhua.TextEditTools.BioCvEdit
{
    partial class BioCvTextEditor
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
            this.rdoDc = new System.Windows.Forms.RadioButton();
            this.chkNgcRdx = new System.Windows.Forms.CheckBox();
            this.chkNgcSysmes = new System.Windows.Forms.CheckBox();
            this.chkDcSysmes = new System.Windows.Forms.CheckBox();
            this.btnCreateFont = new System.Windows.Forms.Button();
            this.btnRdx = new System.Windows.Forms.Button();
            this.pnlCommand.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCommand
            // 
            this.pnlCommand.Controls.Add(this.btnRdx);
            this.pnlCommand.Controls.Add(this.btnCreateFont);
            this.pnlCommand.Controls.Add(this.chkDcSysmes);
            this.pnlCommand.Controls.Add(this.chkNgcSysmes);
            this.pnlCommand.Controls.Add(this.chkNgcRdx);
            this.pnlCommand.Controls.Add(this.rdoDc);
            this.pnlCommand.Controls.Add(this.rdoNgc);
            this.pnlCommand.Location = new System.Drawing.Point(0, 550);
            this.pnlCommand.Controls.SetChildIndex(this.rdoNgc, 0);
            this.pnlCommand.Controls.SetChildIndex(this.rdoDc, 0);
            this.pnlCommand.Controls.SetChildIndex(this.chkNgcRdx, 0);
            this.pnlCommand.Controls.SetChildIndex(this.chkNgcSysmes, 0);
            this.pnlCommand.Controls.SetChildIndex(this.chkDcSysmes, 0);
            this.pnlCommand.Controls.SetChildIndex(this.btnCreateFont, 0);
            this.pnlCommand.Controls.SetChildIndex(this.btnRdx, 0);
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Size = new System.Drawing.Size(912, 584);
            // 
            // rdoNgc
            // 
            this.rdoNgc.AutoSize = true;
            this.rdoNgc.Checked = true;
            this.rdoNgc.Location = new System.Drawing.Point(13, 9);
            this.rdoNgc.Name = "rdoNgc";
            this.rdoNgc.Size = new System.Drawing.Size(43, 16);
            this.rdoNgc.TabIndex = 7;
            this.rdoNgc.TabStop = true;
            this.rdoNgc.Text = "Ngc";
            this.rdoNgc.UseVisualStyleBackColor = true;
            this.rdoNgc.CheckedChanged += new System.EventHandler(this.rdoNgc_CheckedChanged);
            // 
            // rdoDc
            // 
            this.rdoDc.AutoSize = true;
            this.rdoDc.Location = new System.Drawing.Point(198, 10);
            this.rdoDc.Name = "rdoDc";
            this.rdoDc.Size = new System.Drawing.Size(37, 16);
            this.rdoDc.TabIndex = 8;
            this.rdoDc.Text = "Dc";
            this.rdoDc.UseVisualStyleBackColor = true;
            // 
            // chkNgcRdx
            // 
            this.chkNgcRdx.AutoSize = true;
            this.chkNgcRdx.Checked = true;
            this.chkNgcRdx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNgcRdx.Location = new System.Drawing.Point(62, 11);
            this.chkNgcRdx.Name = "chkNgcRdx";
            this.chkNgcRdx.Size = new System.Drawing.Size(44, 16);
            this.chkNgcRdx.TabIndex = 9;
            this.chkNgcRdx.Text = "Rdx";
            this.chkNgcRdx.UseVisualStyleBackColor = true;
            // 
            // chkNgcSysmes
            // 
            this.chkNgcSysmes.AutoSize = true;
            this.chkNgcSysmes.Location = new System.Drawing.Point(112, 10);
            this.chkNgcSysmes.Name = "chkNgcSysmes";
            this.chkNgcSysmes.Size = new System.Drawing.Size(64, 16);
            this.chkNgcSysmes.TabIndex = 10;
            this.chkNgcSysmes.Text = "Sysmes";
            this.chkNgcSysmes.UseVisualStyleBackColor = true;
            // 
            // chkDcSysmes
            // 
            this.chkDcSysmes.AutoSize = true;
            this.chkDcSysmes.Location = new System.Drawing.Point(236, 10);
            this.chkDcSysmes.Name = "chkDcSysmes";
            this.chkDcSysmes.Size = new System.Drawing.Size(64, 16);
            this.chkDcSysmes.TabIndex = 12;
            this.chkDcSysmes.Text = "Sysmes";
            this.chkDcSysmes.UseVisualStyleBackColor = true;
            // 
            // btnCreateFont
            // 
            this.btnCreateFont.Location = new System.Drawing.Point(315, 4);
            this.btnCreateFont.Name = "btnCreateFont";
            this.btnCreateFont.Size = new System.Drawing.Size(60, 26);
            this.btnCreateFont.TabIndex = 15;
            this.btnCreateFont.Text = "画字库";
            this.btnCreateFont.UseVisualStyleBackColor = true;
            this.btnCreateFont.Click += new System.EventHandler(this.btnCreateFont_Click);
            // 
            // btnRdx
            // 
            this.btnRdx.Location = new System.Drawing.Point(381, 4);
            this.btnRdx.Name = "btnRdx";
            this.btnRdx.Size = new System.Drawing.Size(60, 26);
            this.btnRdx.TabIndex = 16;
            this.btnRdx.Text = "合成Rdx";
            this.btnRdx.UseVisualStyleBackColor = true;
            this.btnRdx.Click += new System.EventHandler(this.btnAfsTool_Click);
            // 
            // BioCvTextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 609);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "BioCvTextEditor";
            this.Text = "文本查看";
            this.pnlCommand.ResumeLayout(false);
            this.pnlCommand.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoNgc;
        private System.Windows.Forms.RadioButton rdoDc;
        private System.Windows.Forms.CheckBox chkNgcRdx;
        private System.Windows.Forms.CheckBox chkNgcSysmes;
        private System.Windows.Forms.CheckBox chkDcSysmes;
        private System.Windows.Forms.Button btnCreateFont;
        private System.Windows.Forms.Button btnRdx;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdoADisk;
        private System.Windows.Forms.RadioButton rdoBDisk;

    }
}
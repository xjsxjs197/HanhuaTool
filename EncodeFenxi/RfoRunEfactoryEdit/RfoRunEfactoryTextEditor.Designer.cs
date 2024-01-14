using Hanhua.Common;
namespace Hanhua.TextEditTools.RfoRunEfactory
{
    partial class RfoRunEfactoryTextEditor
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
            this.btnFont = new System.Windows.Forms.Button();
            this.pnlCommand.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCommand
            // 
            this.pnlCommand.Controls.Add(this.btnFont);
            this.pnlCommand.Location = new System.Drawing.Point(0, 550);
            this.pnlCommand.Controls.SetChildIndex(this.btnFont, 0);
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Size = new System.Drawing.Size(912, 584);
            // 
            // btnFont
            // 
            this.btnFont.Location = new System.Drawing.Point(354, 4);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(60, 26);
            this.btnFont.TabIndex = 7;
            this.btnFont.Text = "字库";
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // TalesOfSymphoniaTextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 609);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "TalesOfSymphoniaTextEditor";
            this.Text = "文本查看";
            this.pnlCommand.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFont;

    }
}
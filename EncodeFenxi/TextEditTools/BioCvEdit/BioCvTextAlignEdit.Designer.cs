namespace Hanhua.TextEditTools.BioCvEdit
{
    partial class BioCvTextAlignEdit
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
            this.picText = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlTopBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picText)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Controls.Add(this.picText);
            this.pnlTopBody.Controls.Add(this.panel1);
            this.pnlTopBody.Size = new System.Drawing.Size(779, 239);
            // 
            // picText
            // 
            this.picText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picText.Location = new System.Drawing.Point(0, 0);
            this.picText.Name = "picText";
            this.picText.Size = new System.Drawing.Size(779, 201);
            this.picText.TabIndex = 0;
            this.picText.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 201);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(779, 38);
            this.panel1.TabIndex = 1;
            // 
            // BioCvTextAlignEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 264);
            this.Name = "BioCvTextAlignEdit";
            this.Text = "BioCv 文本对齐工具";
            this.pnlTopBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picText)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picText;
        private System.Windows.Forms.Panel panel1;

    }
}
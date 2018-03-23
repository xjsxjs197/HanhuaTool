namespace Hanhua.BioTools.Bio2Edit
{
    partial class Bio2MovTxtEditor
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
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lstMov = new System.Windows.Forms.ListBox();
            this.imgJp = new System.Windows.Forms.PictureBox();
            this.imgCn = new System.Windows.Forms.PictureBox();
            this.txtCn = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlTopBody.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgJp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCn)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Controls.Add(this.btnSave);
            this.pnlTopBody.Controls.Add(this.txtCn);
            this.pnlTopBody.Controls.Add(this.imgCn);
            this.pnlTopBody.Controls.Add(this.imgJp);
            this.pnlTopBody.Controls.Add(this.pnlLeft);
            this.pnlTopBody.Size = new System.Drawing.Size(638, 216);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.lstMov);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(180, 216);
            this.pnlLeft.TabIndex = 0;
            // 
            // lstMov
            // 
            this.lstMov.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstMov.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMov.FormattingEnabled = true;
            this.lstMov.ItemHeight = 12;
            this.lstMov.Location = new System.Drawing.Point(0, 0);
            this.lstMov.Name = "lstMov";
            this.lstMov.Size = new System.Drawing.Size(180, 206);
            this.lstMov.TabIndex = 0;
            this.lstMov.SelectedIndexChanged += new System.EventHandler(this.lstMov_SelectedIndexChanged);
            // 
            // imgJp
            // 
            this.imgJp.Dock = System.Windows.Forms.DockStyle.Top;
            this.imgJp.Location = new System.Drawing.Point(180, 0);
            this.imgJp.Name = "imgJp";
            this.imgJp.Size = new System.Drawing.Size(458, 40);
            this.imgJp.TabIndex = 1;
            this.imgJp.TabStop = false;
            // 
            // imgCn
            // 
            this.imgCn.Location = new System.Drawing.Point(180, 59);
            this.imgCn.Name = "imgCn";
            this.imgCn.Size = new System.Drawing.Size(458, 40);
            this.imgCn.TabIndex = 2;
            this.imgCn.TabStop = false;
            // 
            // txtCn
            // 
            this.txtCn.BackColor = System.Drawing.Color.Black;
            this.txtCn.Font = new System.Drawing.Font("LiSu", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCn.ForeColor = System.Drawing.Color.White;
            this.txtCn.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtCn.Location = new System.Drawing.Point(204, 115);
            this.txtCn.Multiline = true;
            this.txtCn.Name = "txtCn";
            this.txtCn.Size = new System.Drawing.Size(233, 40);
            this.txtCn.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(357, 174);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 22);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Bio2MovTxtEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 241);
            this.Name = "Bio2MovTxtEditor";
            this.Text = "Bio2MovTxtEditor";
            this.pnlTopBody.ResumeLayout(false);
            this.pnlTopBody.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgJp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.ListBox lstMov;
        private System.Windows.Forms.PictureBox imgJp;
        private System.Windows.Forms.PictureBox imgCn;
        private System.Windows.Forms.TextBox txtCn;
        private System.Windows.Forms.Button btnSave;
    }
}
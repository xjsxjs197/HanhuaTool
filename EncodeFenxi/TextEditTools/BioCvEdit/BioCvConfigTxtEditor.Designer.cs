namespace Hanhua.BioTools.BioCvEdit
{
    partial class BioCvConfigTxtEditor
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
            this.imgConfig = new System.Windows.Forms.PictureBox();
            this.imgJp = new System.Windows.Forms.PictureBox();
            this.imgCn = new System.Windows.Forms.PictureBox();
            this.txtCn = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.txtY = new System.Windows.Forms.TextBox();
            this.txtW = new System.Windows.Forms.TextBox();
            this.txtH = new System.Windows.Forms.TextBox();
            this.btnRe = new System.Windows.Forms.Button();
            this.cmbPosInfo = new System.Windows.Forms.ComboBox();
            this.pnlTopBody.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgJp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCn)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Controls.Add(this.cmbPosInfo);
            this.pnlTopBody.Controls.Add(this.btnRe);
            this.pnlTopBody.Controls.Add(this.txtH);
            this.pnlTopBody.Controls.Add(this.txtW);
            this.pnlTopBody.Controls.Add(this.txtY);
            this.pnlTopBody.Controls.Add(this.txtX);
            this.pnlTopBody.Controls.Add(this.label4);
            this.pnlTopBody.Controls.Add(this.label3);
            this.pnlTopBody.Controls.Add(this.label2);
            this.pnlTopBody.Controls.Add(this.label1);
            this.pnlTopBody.Controls.Add(this.btnSave);
            this.pnlTopBody.Controls.Add(this.txtCn);
            this.pnlTopBody.Controls.Add(this.imgCn);
            this.pnlTopBody.Controls.Add(this.imgJp);
            this.pnlTopBody.Controls.Add(this.pnlLeft);
            this.pnlTopBody.Size = new System.Drawing.Size(638, 216);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.imgConfig);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(180, 216);
            this.pnlLeft.TabIndex = 0;
            // 
            // imgConfig
            // 
            this.imgConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgConfig.Location = new System.Drawing.Point(0, 0);
            this.imgConfig.Name = "imgConfig";
            this.imgConfig.Size = new System.Drawing.Size(180, 216);
            this.imgConfig.TabIndex = 0;
            this.imgConfig.TabStop = false;
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
            this.txtCn.Font = new System.Drawing.Font("隶书", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCn.ForeColor = System.Drawing.Color.White;
            this.txtCn.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtCn.Location = new System.Drawing.Point(186, 113);
            this.txtCn.Multiline = true;
            this.txtCn.Name = "txtCn";
            this.txtCn.Size = new System.Drawing.Size(200, 40);
            this.txtCn.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(216, 180);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 22);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(505, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "X:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(505, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Y:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(484, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Width:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(479, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "Height:";
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(534, 111);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(83, 21);
            this.txtX.TabIndex = 9;
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(534, 137);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(83, 21);
            this.txtY.TabIndex = 10;
            // 
            // txtW
            // 
            this.txtW.Location = new System.Drawing.Point(534, 162);
            this.txtW.Name = "txtW";
            this.txtW.Size = new System.Drawing.Size(83, 21);
            this.txtW.TabIndex = 11;
            // 
            // txtH
            // 
            this.txtH.Location = new System.Drawing.Point(534, 187);
            this.txtH.Name = "txtH";
            this.txtH.Size = new System.Drawing.Size(83, 21);
            this.txtH.TabIndex = 12;
            // 
            // btnRe
            // 
            this.btnRe.Location = new System.Drawing.Point(337, 180);
            this.btnRe.Name = "btnRe";
            this.btnRe.Size = new System.Drawing.Size(100, 22);
            this.btnRe.TabIndex = 13;
            this.btnRe.Text = "刷新";
            this.btnRe.UseVisualStyleBackColor = true;
            this.btnRe.Click += new System.EventHandler(this.btnRe_Click);
            // 
            // cmbPosInfo
            // 
            this.cmbPosInfo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPosInfo.FormattingEnabled = true;
            this.cmbPosInfo.Location = new System.Drawing.Point(421, 116);
            this.cmbPosInfo.Name = "cmbPosInfo";
            this.cmbPosInfo.Size = new System.Drawing.Size(53, 20);
            this.cmbPosInfo.TabIndex = 14;
            this.cmbPosInfo.SelectedIndexChanged += new System.EventHandler(this.cmbPosInfo_SelectedIndexChanged);
            // 
            // BioCvConfigTxtEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 241);
            this.Name = "BioCvConfigTxtEditor";
            this.Text = "Bio2MovTxtEditor";
            this.pnlTopBody.ResumeLayout(false);
            this.pnlTopBody.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgJp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.PictureBox imgJp;
        private System.Windows.Forms.PictureBox imgCn;
        private System.Windows.Forms.TextBox txtCn;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.PictureBox imgConfig;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtH;
        private System.Windows.Forms.TextBox txtW;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Button btnRe;
        private System.Windows.Forms.ComboBox cmbPosInfo;
    }
}
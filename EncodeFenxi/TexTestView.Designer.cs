namespace Hanhua.Common
{
    partial class TexTestView
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.gdvLog = new System.Windows.Forms.DataGridView();
            this.noCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imgCol = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnCeckInfo = new System.Windows.Forms.DataGridViewButtonColumn();
            this.texPosCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdvLog)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gdvLog);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1214, 253);
            this.panel1.TabIndex = 0;
            // 
            // gdvLog
            // 
            this.gdvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gdvLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.noCol,
            this.textCol,
            this.imgCol,
            this.btnCeckInfo,
            this.texPosCol});
            this.gdvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdvLog.Location = new System.Drawing.Point(0, 0);
            this.gdvLog.Name = "gdvLog";
            this.gdvLog.RowHeadersVisible = false;
            this.gdvLog.RowTemplate.Height = 21;
            this.gdvLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gdvLog.Size = new System.Drawing.Size(1214, 253);
            this.gdvLog.TabIndex = 0;
            // 
            // noCol
            // 
            this.noCol.HeaderText = "No";
            this.noCol.Name = "noCol";
            this.noCol.ReadOnly = true;
            this.noCol.Width = 50;
            // 
            // textCol
            // 
            this.textCol.HeaderText = "Log Text";
            this.textCol.Name = "textCol";
            this.textCol.ReadOnly = true;
            this.textCol.Width = 400;
            // 
            // imgCol
            // 
            this.imgCol.HeaderText = "纹理";
            this.imgCol.Name = "imgCol";
            this.imgCol.ReadOnly = true;
            this.imgCol.Width = 640;
            // 
            // btnCeckInfo
            // 
            this.btnCeckInfo.HeaderText = "CheckInfo";
            this.btnCeckInfo.Name = "btnCeckInfo";
            // 
            // texPosCol
            // 
            this.texPosCol.HeaderText = "TexPos";
            this.texPosCol.Name = "texPosCol";
            this.texPosCol.Visible = false;
            // 
            // TexTestView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1214, 253);
            this.Controls.Add(this.panel1);
            this.Name = "TexTestView";
            this.Text = "TexTestView";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gdvLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView gdvLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn noCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn textCol;
        private System.Windows.Forms.DataGridViewImageColumn imgCol;
        private System.Windows.Forms.DataGridViewButtonColumn btnCeckInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn texPosCol;
    }
}
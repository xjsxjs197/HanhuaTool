namespace Hanhua.FileViewer
{
    partial class ImageViewer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tplImageView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.panelParam = new System.Windows.Forms.Panel();
            this.btnAddPic = new System.Windows.Forms.Button();
            this.btnMerge = new System.Windows.Forms.Button();
            this.txtY = new System.Windows.Forms.TextBox();
            this.txtX = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReMerge = new System.Windows.Forms.Button();
            this.btnSelImg = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tplImageView)).BeginInit();
            this.panelParam.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopBody
            // 
            this.pnlTopBody.Size = new System.Drawing.Size(491, 381);
            // 
            // tplImageView
            // 
            this.tplImageView.AllowDrop = true;
            this.tplImageView.AllowUserToAddRows = false;
            this.tplImageView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tplImageView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.tplImageView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tplImageView.ColumnHeadersVisible = false;
            this.tplImageView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tplImageView.DefaultCellStyle = dataGridViewCellStyle5;
            this.tplImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tplImageView.Location = new System.Drawing.Point(0, 0);
            this.tplImageView.Name = "tplImageView";
            this.tplImageView.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tplImageView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.tplImageView.RowHeadersVisible = false;
            this.tplImageView.RowTemplate.Height = 11;
            this.tplImageView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tplImageView.ShowCellErrors = false;
            this.tplImageView.ShowRowErrors = false;
            this.tplImageView.Size = new System.Drawing.Size(491, 381);
            this.tplImageView.TabIndex = 1;
            this.tplImageView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.tplImageView_CellMouseUp);
            this.tplImageView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.tplImageView_CellMouseDown);
            this.tplImageView.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.tplImageView_CellMouseMove);
            this.tplImageView.DragOver += new System.Windows.Forms.DragEventHandler(this.tplImageView_DragOver);
            this.tplImageView.DragDrop += new System.Windows.Forms.DragEventHandler(this.tplImageView_DragDrop);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 256;
            // 
            // panelParam
            // 
            this.panelParam.Controls.Add(this.btnSelImg);
            this.panelParam.Controls.Add(this.btnReMerge);
            this.panelParam.Controls.Add(this.btnAddPic);
            this.panelParam.Controls.Add(this.btnMerge);
            this.panelParam.Controls.Add(this.txtY);
            this.panelParam.Controls.Add(this.txtX);
            this.panelParam.Controls.Add(this.label3);
            this.panelParam.Controls.Add(this.label2);
            this.panelParam.Controls.Add(this.label1);
            this.panelParam.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelParam.Location = new System.Drawing.Point(0, 318);
            this.panelParam.Name = "panelParam";
            this.panelParam.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.panelParam.Size = new System.Drawing.Size(491, 63);
            this.panelParam.TabIndex = 2;
            this.panelParam.Visible = false;
            // 
            // btnAddPic
            // 
            this.btnAddPic.Location = new System.Drawing.Point(187, 33);
            this.btnAddPic.Name = "btnAddPic";
            this.btnAddPic.Size = new System.Drawing.Size(92, 25);
            this.btnAddPic.TabIndex = 6;
            this.btnAddPic.Text = "追加合并图片";
            this.btnAddPic.UseVisualStyleBackColor = true;
            this.btnAddPic.Click += new System.EventHandler(this.btnAddPic_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(289, 5);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(92, 25);
            this.btnMerge.TabIndex = 5;
            this.btnMerge.Text = "继续合成";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // txtY
            // 
            this.txtY.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtY.Location = new System.Drawing.Point(125, 33);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(56, 21);
            this.txtY.TabIndex = 4;
            this.txtY.Text = "0";
            this.txtY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtX
            // 
            this.txtX.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtX.Location = new System.Drawing.Point(125, 8);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(56, 21);
            this.txtX.TabIndex = 3;
            this.txtX.Text = "0";
            this.txtX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(96, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Y：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(96, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "X：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "图片合成：";
            // 
            // btnReMerge
            // 
            this.btnReMerge.Location = new System.Drawing.Point(289, 33);
            this.btnReMerge.Name = "btnReMerge";
            this.btnReMerge.Size = new System.Drawing.Size(92, 25);
            this.btnReMerge.TabIndex = 7;
            this.btnReMerge.Text = "重新合成";
            this.btnReMerge.UseVisualStyleBackColor = true;
            this.btnReMerge.Click += new System.EventHandler(this.btnReMerge_Click);
            // 
            // btnSelImg
            // 
            this.btnSelImg.Location = new System.Drawing.Point(187, 5);
            this.btnSelImg.Name = "btnSelImg";
            this.btnSelImg.Size = new System.Drawing.Size(92, 25);
            this.btnSelImg.TabIndex = 8;
            this.btnSelImg.Text = "重新选择图片";
            this.btnSelImg.UseVisualStyleBackColor = true;
            this.btnSelImg.Click += new System.EventHandler(this.btnSelImg_Click);
            // 
            // ImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 406);
            this.Controls.Add(this.panelParam);
            this.Controls.Add(this.tplImageView);
            this.Name = "ImageViewer";
            this.Text = "TplImage";
            this.Controls.SetChildIndex(this.pnlTopBody, 0);
            this.Controls.SetChildIndex(this.tplImageView, 0);
            this.Controls.SetChildIndex(this.panelParam, 0);
            ((System.ComponentModel.ISupportInitialize)(this.tplImageView)).EndInit();
            this.panelParam.ResumeLayout(false);
            this.panelParam.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView tplImageView;
        private System.Windows.Forms.DataGridViewImageColumn Column1;
        private System.Windows.Forms.Panel panelParam;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Button btnAddPic;
        private System.Windows.Forms.Button btnReMerge;
        private System.Windows.Forms.Button btnSelImg;
    }
}
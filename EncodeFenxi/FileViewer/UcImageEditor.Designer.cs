namespace Hanhua.FileViewer
{
    partial class UcImageEditor
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelParam = new System.Windows.Forms.Panel();
            this.btnMerge = new System.Windows.Forms.Button();
            this.txtY = new System.Windows.Forms.TextBox();
            this.txtX = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tplImageView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.panelParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tplImageView)).BeginInit();
            this.SuspendLayout();
            // 
            // panelParam
            // 
            this.panelParam.Controls.Add(this.btnMerge);
            this.panelParam.Controls.Add(this.txtY);
            this.panelParam.Controls.Add(this.txtX);
            this.panelParam.Controls.Add(this.label3);
            this.panelParam.Controls.Add(this.label2);
            this.panelParam.Controls.Add(this.label1);
            this.panelParam.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelParam.Location = new System.Drawing.Point(0, 283);
            this.panelParam.Name = "panelParam";
            this.panelParam.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.panelParam.Size = new System.Drawing.Size(430, 33);
            this.panelParam.TabIndex = 3;
            this.panelParam.Visible = false;
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(350, 7);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(67, 20);
            this.btnMerge.TabIndex = 5;
            this.btnMerge.Text = "开始合成";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // txtY
            // 
            this.txtY.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtY.Location = new System.Drawing.Point(268, 8);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(56, 21);
            this.txtY.TabIndex = 4;
            this.txtY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtX
            // 
            this.txtX.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtX.Location = new System.Drawing.Point(161, 8);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(56, 21);
            this.txtX.TabIndex = 3;
            this.txtX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(239, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Y：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 11);
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
            // tplImageView
            // 
            this.tplImageView.AllowDrop = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tplImageView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tplImageView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tplImageView.ColumnHeadersVisible = false;
            this.tplImageView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tplImageView.DefaultCellStyle = dataGridViewCellStyle2;
            this.tplImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tplImageView.Location = new System.Drawing.Point(0, 0);
            this.tplImageView.Name = "tplImageView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tplImageView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.tplImageView.RowHeadersVisible = false;
            this.tplImageView.RowTemplate.Height = 11;
            this.tplImageView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tplImageView.ShowCellErrors = false;
            this.tplImageView.ShowRowErrors = false;
            this.tplImageView.Size = new System.Drawing.Size(430, 283);
            this.tplImageView.TabIndex = 4;
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
            this.Column1.Width = 256;
            // 
            // UcImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tplImageView);
            this.Controls.Add(this.panelParam);
            this.Name = "UcImageViewer";
            this.Size = new System.Drawing.Size(430, 316);
            this.panelParam.ResumeLayout(false);
            this.panelParam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tplImageView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelParam;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView tplImageView;
        private System.Windows.Forms.DataGridViewImageColumn Column1;
    }
}

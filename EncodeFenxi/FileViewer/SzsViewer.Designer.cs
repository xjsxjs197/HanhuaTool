namespace Hanhua.FileViewer
{
    partial class SzsViewer
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
            this.fileInfoTree = new System.Windows.Forms.TreeView();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.bodyPanel = new System.Windows.Forms.Panel();
            this.pnlLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.fileInfoTree);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.pnlLeft.Size = new System.Drawing.Size(208, 463);
            this.pnlLeft.TabIndex = 0;
            // 
            // fileInfoTree
            // 
            this.fileInfoTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileInfoTree.Location = new System.Drawing.Point(0, 0);
            this.fileInfoTree.Name = "fileInfoTree";
            this.fileInfoTree.Size = new System.Drawing.Size(204, 463);
            this.fileInfoTree.TabIndex = 0;
            this.fileInfoTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.fileInfoTree_AfterSelect);
            this.fileInfoTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.fileInfoTree_NodeMouseClick);
            // 
            // pnlRight
            // 
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(208, 0);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(5);
            this.pnlRight.Size = new System.Drawing.Size(372, 463);
            this.pnlRight.TabIndex = 1;
            this.pnlRight.Visible = false;
            // 
            // bodyPanel
            // 
            this.bodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bodyPanel.Location = new System.Drawing.Point(0, 0);
            this.bodyPanel.Name = "bodyPanel";
            this.bodyPanel.Size = new System.Drawing.Size(580, 463);
            this.bodyPanel.TabIndex = 3;
            // 
            // SzsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 463);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.bodyPanel);
            this.Name = "SzsViewer";
            this.Text = "SzsViewer";
            this.Controls.SetChildIndex(this.bodyPanel, 0);
            this.Controls.SetChildIndex(this.pnlLeft, 0);
            this.Controls.SetChildIndex(this.pnlRight, 0);
            this.pnlLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.TreeView fileInfoTree;
        private System.Windows.Forms.Panel bodyPanel;
    }
}
namespace OpenORPG.Toolkit.Views.Content
{
    partial class DialogEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogEditor));
            this.treeDialog = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.contextDialogMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addLinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.removeSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.contextDialogMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.treeDialog);
            this.panel1.Size = new System.Drawing.Size(1066, 466);
            // 
            // treeDialog
            // 
            this.treeDialog.ContextMenuStrip = this.contextDialogMenu;
            this.treeDialog.Dock = System.Windows.Forms.DockStyle.Right;
            this.treeDialog.ImageKey = "script.png";
            this.treeDialog.ImageList = this.imageList1;
            this.treeDialog.Location = new System.Drawing.Point(251, 0);
            this.treeDialog.Name = "treeDialog";
            this.treeDialog.SelectedImageIndex = 0;
            this.treeDialog.Size = new System.Drawing.Size(815, 466);
            this.treeDialog.TabIndex = 0;
            this.treeDialog.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeDialog_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "script.png");
            this.imageList1.Images.SetKeyName(1, "link.png");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtText);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtComment);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 179);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(9, 64);
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(218, 109);
            this.txtText.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Text:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Comment:";
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(66, 19);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(161, 20);
            this.txtComment.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(12, 204);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(233, 256);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Properties";
            // 
            // contextDialogMenu
            // 
            this.contextDialogMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLinkToolStripMenuItem,
            this.addNodeToolStripMenuItem,
            this.toolStripSeparator1,
            this.removeSelectedToolStripMenuItem});
            this.contextDialogMenu.Name = "contextDialogMenu";
            this.contextDialogMenu.Size = new System.Drawing.Size(165, 76);
            // 
            // addLinkToolStripMenuItem
            // 
            this.addLinkToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addLinkToolStripMenuItem.Image")));
            this.addLinkToolStripMenuItem.Name = "addLinkToolStripMenuItem";
            this.addLinkToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.addLinkToolStripMenuItem.Text = "Add Link";
            this.addLinkToolStripMenuItem.Click += new System.EventHandler(this.addLinkToolStripMenuItem_Click);
            // 
            // addNodeToolStripMenuItem
            // 
            this.addNodeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addNodeToolStripMenuItem.Image")));
            this.addNodeToolStripMenuItem.Name = "addNodeToolStripMenuItem";
            this.addNodeToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.addNodeToolStripMenuItem.Text = "Add Node";
            this.addNodeToolStripMenuItem.Click += new System.EventHandler(this.addNodeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(161, 6);
            // 
            // removeSelectedToolStripMenuItem
            // 
            this.removeSelectedToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeSelectedToolStripMenuItem.Image")));
            this.removeSelectedToolStripMenuItem.Name = "removeSelectedToolStripMenuItem";
            this.removeSelectedToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.removeSelectedToolStripMenuItem.Text = "Remove Selected";
            this.removeSelectedToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedToolStripMenuItem_Click);
            // 
            // DialogEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 495);
            this.Name = "DialogEditor";
            this.Text = "DialougeEditor";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextDialogMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView treeDialog;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextDialogMenu;
        private System.Windows.Forms.ToolStripMenuItem addLinkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem removeSelectedToolStripMenuItem;
    }
}
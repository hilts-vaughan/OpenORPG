namespace OpenORPG.Toolkit.Views
{
    partial class ContentSelectionForm
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
            System.Windows.Forms.ImageList ImageList;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContentSelectionForm));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.contextAddContent = new System.Windows.Forms.ToolStripMenuItem();
            ImageList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImageList
            // 
            ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            ImageList.TransparentColor = System.Drawing.Color.Transparent;
            ImageList.Images.SetKeyName(0, "folder.png");
            ImageList.Images.SetKeyName(1, "page_lightning.png");
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenu;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageKey = "folder.png";
            this.treeView1.ImageList = ImageList;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(617, 418);
            this.treeView1.TabIndex = 3;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextAddFolder,
            this.contextAddContent});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(143, 48);
            // 
            // contextAddFolder
            // 
            this.contextAddFolder.Image = ((System.Drawing.Image)(resources.GetObject("contextAddFolder.Image")));
            this.contextAddFolder.Name = "contextAddFolder";
            this.contextAddFolder.Size = new System.Drawing.Size(152, 22);
            this.contextAddFolder.Text = "Add Folder";
            this.contextAddFolder.Click += new System.EventHandler(this.contextAddFolder_Click);
            // 
            // contextAddContent
            // 
            this.contextAddContent.Image = ((System.Drawing.Image)(resources.GetObject("contextAddContent.Image")));
            this.contextAddContent.Name = "contextAddContent";
            this.contextAddContent.Size = new System.Drawing.Size(152, 22);
            this.contextAddContent.Text = "Add Content";
            this.contextAddContent.Click += new System.EventHandler(this.contextAddContent_Click);
            // 
            // ContentSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 418);
            this.ControlBox = false;
            this.Controls.Add(this.treeView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ContentSelectionForm";
            this.ShowInTaskbar = false;
            this.Text = "Select content...";
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem contextAddFolder;
        private System.Windows.Forms.ToolStripMenuItem contextAddContent;
    }
}
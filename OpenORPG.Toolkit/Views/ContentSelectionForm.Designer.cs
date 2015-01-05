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
            this.listContent = new System.Windows.Forms.ListBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            ImageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // listContent
            // 
            this.listContent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listContent.FormattingEnabled = true;
            this.listContent.Location = new System.Drawing.Point(0, 388);
            this.listContent.Name = "listContent";
            this.listContent.Size = new System.Drawing.Size(617, 30);
            this.listContent.TabIndex = 1;
            this.listContent.DoubleClick += new System.EventHandler(this.listContent_DoubleClick);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageKey = "folder.png";
            this.treeView1.ImageList = ImageList;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(617, 388);
            this.treeView1.TabIndex = 3;
            // 
            // ImageList
            // 
            ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            ImageList.TransparentColor = System.Drawing.Color.Transparent;
            ImageList.Images.SetKeyName(0, "folder.png");
            ImageList.Images.SetKeyName(1, "page_lightning.png");
            // 
            // ContentSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 418);
            this.ControlBox = false;
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.listContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ContentSelectionForm";
            this.ShowInTaskbar = false;
            this.Text = "Select content...";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listContent;
        private System.Windows.Forms.TreeView treeView1;
    }
}
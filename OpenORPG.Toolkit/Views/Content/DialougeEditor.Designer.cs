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
            this.contextDialogMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addLinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.removeSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textScript = new System.Windows.Forms.TextBox();
            this.txtText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.groupConditions = new System.Windows.Forms.GroupBox();
            this.listConditions = new System.Windows.Forms.ListBox();
            this.contextConditionMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addConditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeConditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.groupActions = new System.Windows.Forms.GroupBox();
            this.listActions = new System.Windows.Forms.ListBox();
            this.contextActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemAddAction = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRemoveAction = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.contextDialogMenu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupConditions.SuspendLayout();
            this.contextConditionMenu.SuspendLayout();
            this.groupActions.SuspendLayout();
            this.contextActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupActions);
            this.panel1.Controls.Add(this.textName);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.groupConditions);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.treeDialog);
            this.panel1.Size = new System.Drawing.Size(1379, 466);
            // 
            // treeDialog
            // 
            this.treeDialog.ContextMenuStrip = this.contextDialogMenu;
            this.treeDialog.Dock = System.Windows.Forms.DockStyle.Right;
            this.treeDialog.ImageKey = "script.png";
            this.treeDialog.ImageList = this.imageList1;
            this.treeDialog.Location = new System.Drawing.Point(918, 0);
            this.treeDialog.Name = "treeDialog";
            this.treeDialog.SelectedImageIndex = 0;
            this.treeDialog.Size = new System.Drawing.Size(461, 466);
            this.treeDialog.TabIndex = 0;
            this.treeDialog.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeDialog_AfterSelect);
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
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "script.png");
            this.imageList1.Images.SetKeyName(1, "link.png");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textScript);
            this.groupBox1.Controls.Add(this.txtText);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtComment);
            this.groupBox1.Location = new System.Drawing.Point(12, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 425);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Script:";
            // 
            // textScript
            // 
            this.textScript.Location = new System.Drawing.Point(66, 47);
            this.textScript.Name = "textScript";
            this.textScript.Size = new System.Drawing.Size(161, 20);
            this.textScript.TabIndex = 6;
            // 
            // txtText
            // 
            this.txtText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtText.Location = new System.Drawing.Point(9, 89);
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(218, 330);
            this.txtText.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 73);
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
            // groupConditions
            // 
            this.groupConditions.Controls.Add(this.listConditions);
            this.groupConditions.Location = new System.Drawing.Point(251, 35);
            this.groupConditions.Name = "groupConditions";
            this.groupConditions.Size = new System.Drawing.Size(254, 425);
            this.groupConditions.TabIndex = 2;
            this.groupConditions.TabStop = false;
            this.groupConditions.Text = "Conditions";
            // 
            // listConditions
            // 
            this.listConditions.ContextMenuStrip = this.contextConditionMenu;
            this.listConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listConditions.FormattingEnabled = true;
            this.listConditions.Location = new System.Drawing.Point(3, 16);
            this.listConditions.Name = "listConditions";
            this.listConditions.Size = new System.Drawing.Size(248, 406);
            this.listConditions.TabIndex = 4;
            // 
            // contextConditionMenu
            // 
            this.contextConditionMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addConditionToolStripMenuItem,
            this.removeConditionToolStripMenuItem});
            this.contextConditionMenu.Name = "contextConditionMenu";
            this.contextConditionMenu.Size = new System.Drawing.Size(174, 48);
            // 
            // addConditionToolStripMenuItem
            // 
            this.addConditionToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addConditionToolStripMenuItem.Image")));
            this.addConditionToolStripMenuItem.Name = "addConditionToolStripMenuItem";
            this.addConditionToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.addConditionToolStripMenuItem.Text = "Add Condition";
            this.addConditionToolStripMenuItem.Click += new System.EventHandler(this.addConditionToolStripMenuItem_Click);
            // 
            // removeConditionToolStripMenuItem
            // 
            this.removeConditionToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeConditionToolStripMenuItem.Image")));
            this.removeConditionToolStripMenuItem.Name = "removeConditionToolStripMenuItem";
            this.removeConditionToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.removeConditionToolStripMenuItem.Text = "Remove Condition";
            this.removeConditionToolStripMenuItem.Click += new System.EventHandler(this.removeConditionToolStripMenuItem_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Dialog Name:";
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(95, 8);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(670, 20);
            this.textName.TabIndex = 4;
            // 
            // groupActions
            // 
            this.groupActions.Controls.Add(this.listActions);
            this.groupActions.Location = new System.Drawing.Point(511, 35);
            this.groupActions.Name = "groupActions";
            this.groupActions.Size = new System.Drawing.Size(254, 425);
            this.groupActions.TabIndex = 5;
            this.groupActions.TabStop = false;
            this.groupActions.Text = "Actions";
            // 
            // listActions
            // 
            this.listActions.ContextMenuStrip = this.contextActions;
            this.listActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listActions.FormattingEnabled = true;
            this.listActions.Location = new System.Drawing.Point(3, 16);
            this.listActions.Name = "listActions";
            this.listActions.Size = new System.Drawing.Size(248, 406);
            this.listActions.TabIndex = 4;
            // 
            // contextActions
            // 
            this.contextActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAddAction,
            this.menuItemRemoveAction});
            this.contextActions.Name = "contextActions";
            this.contextActions.Size = new System.Drawing.Size(156, 48);
            // 
            // menuItemAddAction
            // 
            this.menuItemAddAction.Image = ((System.Drawing.Image)(resources.GetObject("menuItemAddAction.Image")));
            this.menuItemAddAction.Name = "menuItemAddAction";
            this.menuItemAddAction.Size = new System.Drawing.Size(155, 22);
            this.menuItemAddAction.Text = "Add Action";
            this.menuItemAddAction.Click += new System.EventHandler(this.menuItemAddAction_Click);
            // 
            // menuItemRemoveAction
            // 
            this.menuItemRemoveAction.Image = ((System.Drawing.Image)(resources.GetObject("menuItemRemoveAction.Image")));
            this.menuItemRemoveAction.Name = "menuItemRemoveAction";
            this.menuItemRemoveAction.Size = new System.Drawing.Size(155, 22);
            this.menuItemRemoveAction.Text = "Remove Action";
            this.menuItemRemoveAction.Click += new System.EventHandler(this.menuItemRemoveAction_Click);
            // 
            // DialogEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1379, 495);
            this.Name = "DialogEditor";
            this.Text = "DialougeEditor";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextDialogMenu.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupConditions.ResumeLayout(false);
            this.contextConditionMenu.ResumeLayout(false);
            this.groupActions.ResumeLayout(false);
            this.contextActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupConditions;
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
        private System.Windows.Forms.ListBox listConditions;
        private System.Windows.Forms.ContextMenuStrip contextConditionMenu;
        private System.Windows.Forms.ToolStripMenuItem addConditionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeConditionToolStripMenuItem;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textScript;
        private System.Windows.Forms.GroupBox groupActions;
        private System.Windows.Forms.ListBox listActions;
        private System.Windows.Forms.ContextMenuStrip contextActions;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddAction;
        private System.Windows.Forms.ToolStripMenuItem menuItemRemoveAction;
    }
}
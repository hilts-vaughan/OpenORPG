namespace OpenORPG.Toolkit.Views.Content.Quests
{
    partial class QuestRequirementsDialog
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
            this.listRequirements = new System.Windows.Forms.ListBox();
            this.contextStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.contextNewRequirement = new System.Windows.Forms.ToolStripMenuItem();
            this.contextRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.contextStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // listRequirements
            // 
            this.listRequirements.Dock = System.Windows.Forms.DockStyle.Left;
            this.listRequirements.FormattingEnabled = true;
            this.listRequirements.Location = new System.Drawing.Point(0, 0);
            this.listRequirements.Name = "listRequirements";
            this.listRequirements.Size = new System.Drawing.Size(209, 267);
            this.listRequirements.TabIndex = 0;
            // 
            // contextStrip
            // 
            this.contextStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextNewRequirement,
            this.contextRemove});
            this.contextStrip.Name = "contextStrip";
            this.contextStrip.Size = new System.Drawing.Size(118, 48);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(209, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(398, 267);
            this.propertyGrid1.TabIndex = 2;
            // 
            // contextNewRequirement
            // 
            this.contextNewRequirement.Name = "contextNewRequirement";
            this.contextNewRequirement.Size = new System.Drawing.Size(152, 22);
            this.contextNewRequirement.Text = "New...";
            // 
            // contextRemove
            // 
            this.contextRemove.Name = "contextRemove";
            this.contextRemove.Size = new System.Drawing.Size(152, 22);
            this.contextRemove.Text = "Remove";
            // 
            // QuestRequirementsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 267);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.listRequirements);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuestRequirementsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quest Step Requirements";
            this.contextStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listRequirements;
        private System.Windows.Forms.ContextMenuStrip contextStrip;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStripMenuItem contextNewRequirement;
        private System.Windows.Forms.ToolStripMenuItem contextRemove;
    }
}
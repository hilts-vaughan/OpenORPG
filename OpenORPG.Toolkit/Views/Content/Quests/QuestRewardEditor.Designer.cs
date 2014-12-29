namespace OpenORPG.Toolkit.Views.Content.Quests
{
    partial class QuestRewardEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextRewards = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.listRewards = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // contextRewards
            // 
            this.contextRewards.Name = "contextRewards";
            this.contextRewards.Size = new System.Drawing.Size(61, 4);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 192);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(380, 220);
            this.propertyGrid1.TabIndex = 3;
            // 
            // listRewards
            // 
            this.listRewards.ContextMenuStrip = this.contextRewards;
            this.listRewards.Dock = System.Windows.Forms.DockStyle.Top;
            this.listRewards.FormattingEnabled = true;
            this.listRewards.Location = new System.Drawing.Point(0, 0);
            this.listRewards.Name = "listRewards";
            this.listRewards.Size = new System.Drawing.Size(380, 186);
            this.listRewards.TabIndex = 2;
            // 
            // QuestRewardEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.listRewards);
            this.Name = "QuestRewardEditor";
            this.Size = new System.Drawing.Size(380, 412);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextRewards;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ListBox listRewards;
    }
}

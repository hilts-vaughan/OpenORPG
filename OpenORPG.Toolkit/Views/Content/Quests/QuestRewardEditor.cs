using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenORPG.Database.Models.Quests.Rewards;

namespace OpenORPG.Toolkit.Views.Content.Quests
{
    public partial class QuestRewardEditor : UserControl
    {
        public QuestRewardEditor()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                //PopulateContextMenu();
            }
        }

      


        private void PopulateContextMenu()
        {
            // We need all the quest rewards to populate our context menu
            var subclasses = ReflectionUtility.GetAllTypesWithSubclass<QuestReward>();

            foreach (var subclass in subclasses)
            {
                var toolstripItem = new ToolStripMenuItem(subclass.Name);
                toolstripItem.Tag = subclass;

                contextRewards.Items.Add(toolstripItem);
            }

        }



    }
}

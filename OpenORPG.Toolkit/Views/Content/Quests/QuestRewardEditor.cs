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
using Server.Game.Database.Models.Quests;

namespace OpenORPG.Toolkit.Views.Content.Quests
{
    public partial class QuestRewardEditor : UserControl
    {
        private QuestTemplate _template;
        private BindingList<QuestReward> _rewards; 

        public QuestRewardEditor()
        {
            InitializeComponent();

            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

            if (!designMode)
                PopulateContextMenu();

        }

        public QuestTemplate Template
        {
            set { _template = value;

                _rewards = new BindingList<QuestReward>();

                foreach (var reward in value.Rewards)
                    _rewards.Add(reward);

                listRewards.DataSource = _rewards;
              
                listRewards.DisplayMember = "DisplayString";
                listRewards.ValueMember = "QuestRewardId";

                listRewards.SelectedIndexChanged += listRewards_SelectedIndexChanged;
            }
        }

        public List<QuestReward> Rewards
        {
            get { return new List<QuestReward>(_rewards);  }
        }

        void listRewards_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the property grid
            propertyGrid1.SelectedObject = _rewards[listRewards.SelectedIndex];
        }


        private void PopulateContextMenu()
        {
            // We need all the quest rewards to populate our context menu
            var subclasses = ReflectionUtility.GetAllTypesWithSubclass<QuestReward>();

            foreach (var subclass in subclasses)
            {
                var toolstripItem = new ToolStripMenuItem(subclass.Name);
                toolstripItem.Tag = subclass;
                toolstripItem.Click += toolstripItem_Click;
                contextRewards.Items.Add(toolstripItem);
            }

        }

        void toolstripItem_Click(object sender, EventArgs e)
        {
            var type = (Type)(sender as ToolStripMenuItem).Tag;
            _rewards.Add((QuestReward) Activator.CreateInstance(type));            
        }



    }
}

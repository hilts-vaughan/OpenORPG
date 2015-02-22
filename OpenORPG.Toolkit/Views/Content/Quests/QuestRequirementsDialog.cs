using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenORPG.Database.Models.Quests;
using OpenORPG.Database.Models.Quests.Rewards;

namespace OpenORPG.Toolkit.Views.Content.Quests
{
    public partial class QuestRequirementsDialog : Form
    {
        private BindingList<QuestRequirement> _requirements; 

        public QuestRequirementsDialog()
        {
            InitializeComponent();

            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

            if (!designMode)
            {
               PopulateContextMenu();
            }

        }

        public List<QuestRequirement> Requirements
        {
            set
            {                
                // Do some binding  
                _requirements = new BindingList<QuestRequirement>(value);

                listRequirements.DataSource = _requirements;
                listRequirements.DisplayMember = "DisplayString";
                listRequirements.ValueMember = "QuestRequirementId";

                listRequirements.SelectedIndexChanged += ListRequirementsOnSelectedIndexChanged;
            }

            get
            {
                return new List<QuestRequirement>(_requirements);
            }
        }



        private void PopulateContextMenu()
        {
            // We need all the quest rewards to populate our context menu
            var subclasses = ReflectionUtility.GetAllTypesWithSubclass<QuestRequirement>();

            foreach (var subclass in subclasses)
            {
                var toolstripItem = new ToolStripMenuItem(subclass.Name);
                toolstripItem.Tag = subclass;
                toolstripItem.Click += ToolstripItemOnClick;
                contextRequirements.Items.Add(toolstripItem);
            }

        }

        private void ToolstripItemOnClick(object sender, EventArgs eventArgs)
        {
            var toolStripMenuItem = sender as ToolStripMenuItem;

            if (toolStripMenuItem != null)
            {
                var type = (Type) toolStripMenuItem.Tag;
                _requirements.Add((QuestRequirement) Activator.CreateInstance(type));
            }
            else
            {
                throw new ArgumentNullException("tag",
                    "The specific tag was not populated with the expected type of QuestRequirement");
            }
        }

        private void ListRequirementsOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            propertyGrid1.SelectedObject = _requirements[listRequirements.SelectedIndex];
        }
    }
}

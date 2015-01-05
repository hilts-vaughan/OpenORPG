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
                //contextNewRequirement
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
        }

        private void ListRequirementsOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            propertyGrid1.SelectedObject = _requirements[listRequirements.SelectedIndex];
        }
    }
}

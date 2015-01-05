using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenORPG.Database.Models.Quests;
using OpenORPG.Database.Models.Quests.Rewards;
using Server.Game.Database.Models.Quests;

namespace OpenORPG.Toolkit.Views.Content.Quests
{
    public partial class QuestStepEditor : UserControl
    {
        private QuestTemplate _template;
        private BindingList<QuestStepsTable> _steps; 


        public QuestStepEditor()
        {
            InitializeComponent();


            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

            if (!designMode)
            {
                
            }


        }

        public List<QuestStepsTable> Steps
        {
            get { return new List<QuestStepsTable>(_steps); }
        }

        public QuestTemplate Template
        {
            set
            {
                _template = value;
                
           
                _steps = new BindingList<QuestStepsTable>();

                foreach (var x in value.QuestSteps)
                    _steps.Add(x);


                listSteps.DataSource = _steps;
                listSteps.DisplayMember = "Name";
                listSteps.ValueMember = "QuestStepsTableId";
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            _steps.Add(new QuestStepsTable() { Name = "New Step"});
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            var dialog = new QuestRequirementsDialog();
            dialog.Requirements = new List<QuestRequirement>(GetCurrentQuestStepsTable().Requirements);
            dialog.ShowDialog();
        }

        private QuestStepsTable GetCurrentQuestStepsTable()
        {
            return _steps[listSteps.SelectedIndex];
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            var step = GetCurrentQuestStepsTable();
            _steps.Remove(step);
        }


    }
}

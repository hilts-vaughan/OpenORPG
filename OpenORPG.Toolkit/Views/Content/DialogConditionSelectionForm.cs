using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenORPG.Common.Dialog.Conditions;

namespace OpenORPG.Toolkit.Views.Content
{
    public partial class DialogConditionSelectionForm : Form
    {

        private class ConditionEntry
        {
            public Type ConditionType { get; private set; }
        }

        public DialogConditionSelectionForm()
        {
            InitializeComponent();
            listConditions.DisplayMember = "Name";
            GenerateOptions();
        }

        public IDialogCondition Condition { get; private set; }

        private void GenerateOptions()
        {
            // We need all the quest rewards to populate our context menu
            var subclasses = ReflectionUtility.GetAllTypesWithSubclass<IDialogCondition>();

            foreach (var subclass in subclasses)
            {
                listConditions.Items.Add(subclass);
            }
        }

        private void listConditions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listConditions.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                var condition = listConditions.Items[index];
                SetupEdit(condition as Type);
            }
        }

        private void SetupEdit(Type type)
        {
            Condition = (IDialogCondition) Activator.CreateInstance(type);
            propertyGrid1.SelectedObject = Condition;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }



    }
}

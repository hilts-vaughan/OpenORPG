using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenORPG.Common.Dialog.Actions;
using OpenORPG.Common.Dialog.Conditions;

namespace OpenORPG.Toolkit.Views.Content
{
    public partial class DialogActionSelectionForm : Form
    {

        private class ActionEntry
        {
            public Type ConditionType { get; private set; }
        }

        public DialogActionSelectionForm()
        {
            InitializeComponent();
            listConditions.DisplayMember = "Name";
            GenerateOptions();
        }

        public IDialogAction Condition { get; private set; }

        private void GenerateOptions()
        {
            // We need all the quest rewards to populate our context menu
            var subclasses = ReflectionUtility.GetAllTypesThatImplement<IDialogAction>();

            foreach (var subclass in subclasses)
            {
                listConditions.Items.Add(subclass);
            }
        }


        private void SetupEdit(Type type)
        {
            Condition = (IDialogAction) Activator.CreateInstance(type);
            propertyGrid1.SelectedObject = Condition;
        }

      

        private void listConditions_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            int index = this.listConditions.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                var condition = listConditions.Items[index];
                SetupEdit(condition as Type);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }



    }
}

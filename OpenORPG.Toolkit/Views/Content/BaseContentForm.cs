using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenORPG.Database.DAL;
using Server.Game.Database;
using Server.Game.Database.Models.ContentTemplates;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenORPG.Toolkit.Views.Content
{
    public partial class BaseContentForm : DockContent
    {

        protected object ContentTemplate;

        public BaseContentForm()
        {
            InitializeComponent();
        }

        public void SetContentTemplate(object template)
        {
            ContentTemplate = template;
            var binding = DataBindings.Add("Text", ContentTemplate, "Name");
            binding.Format += BindingOnFormat;
        }

        private void BindingOnFormat(object sender, ConvertEventArgs convertEventArgs)
        {
            convertEventArgs.Value = ContentTemplate.GetType().Name + " - " + ContentTemplate;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Save();
            Close();
        }

        protected virtual void Save()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}

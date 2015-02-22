using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenORPG.Common.Dialog;
using OpenORPG.Database.Models.ContentTemplates;
using OpenORPG.Toolkit.Views.Authentication;
using OpenORPG.Toolkit.Views.Content;
using Server.Game.Database.Models;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Database.Models.Quests;
using Server.Infrastructure.Dialog;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenORPG.Toolkit.Views
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        
            dockPanel1.Theme = new VS2012LightTheme();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {


        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            var loginForm = new LoginForm();
            loginForm.ShowDialog();          
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void monstersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var template = (MonsterTemplate) PresentContentForm(typeof(MonsterTemplate));
            var monster = new MonsterEditorForm(template);
            monster.Show(dockPanel1);
        }

        private IContentTemplate PresentContentForm(Type type)
        {
            var form = new ContentSelectionForm(type);
            form.ShowDialog();
            return form.SelectedTemplate;
        }

        private void itemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var template = (ItemTemplate) PresentContentForm(typeof(ItemTemplate));
            var monster = new ItemEditorForm(template);
            monster.Show(dockPanel1);
        }

        private void skillsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var template = (SkillTemplate)PresentContentForm(typeof(SkillTemplate));
            var monster = new SkillEditorForm(template);
            monster.Show(dockPanel1);
        }

        private void questsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var template = (QuestTemplate)PresentContentForm(typeof(QuestTemplate));
            var monster = new QuestEditorForm(template);
            monster.Show(dockPanel1);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var template = (NpcTemplate) PresentContentForm(typeof(NpcTemplate));
            var monster = new NpcEditorForm(template);
            monster.Show(dockPanel1);
        }

        private void dialougeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var template = (DialogTemplate)PresentContentForm(typeof(DialogTemplate));
            var form = new DialogEditor(template);
            form.Show(dockPanel1);
        }



    }
}

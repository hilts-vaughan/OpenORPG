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

namespace OpenORPG.Toolkit.Views
{
    public partial class MonsterEditorForm : DockContent
    {
        // The currently stored template
        private MonsterTemplate _monsterTemplate;

        public MonsterEditorForm(MonsterTemplate monsterTemplate)
        {
            InitializeComponent();

            _monsterTemplate = monsterTemplate;

            SetupBindings();

        }

        private void SetupBindings()
        {
            textNotes.DataBindings.Add("Text", _monsterTemplate, "Notes");
            textName.DataBindings.Add("Text", _monsterTemplate, "Name");
            textDescription.DataBindings.Add("Text", _monsterTemplate, "Description");
            numericLevel.DataBindings.Add("Value", _monsterTemplate, "Level");
        }

        private void MonsterEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new GameDatabaseContext())
            {
                var repository = new MonsterRepository(db);
                repository.Update(_monsterTemplate, _monsterTemplate.Id);
            }

            Close();
        }
  


    }
}

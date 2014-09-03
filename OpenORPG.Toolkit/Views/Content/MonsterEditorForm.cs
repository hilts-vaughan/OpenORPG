using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenORPG.Database.DAL;
using Server.Game.Database;
using Server.Game.Database.Models.ContentTemplates;

namespace OpenORPG.Toolkit.Views.Content
{
    public partial class MonsterEditorForm : BaseContentForm
    {


        public MonsterEditorForm()
        {
            InitializeComponent();
        }

        public MonsterEditorForm(MonsterTemplate template)   
        {
            InitializeComponent();

            SetContentTemplate(template);

            textNotes.DataBindings.Add("Text", ContentTemplate, "Notes");
            textName.DataBindings.Add("Text", ContentTemplate, "Name");
            textDescription.DataBindings.Add("Text", ContentTemplate, "Description");
            numericLevel.DataBindings.Add("Value", ContentTemplate, "Level");

        }

        protected override void Save()
        {
            using (var db = new GameDatabaseContext())
            {
                var ContentTemplate = this.ContentTemplate as MonsterTemplate;
                var repository = new MonsterRepository(db);
                repository.Update(ContentTemplate, ContentTemplate.Id);
            }           
        
            base.Save();
        }


    }
}

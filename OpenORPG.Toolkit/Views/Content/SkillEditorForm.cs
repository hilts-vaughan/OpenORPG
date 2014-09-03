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
    public partial class SkillEditorForm : OpenORPG.Toolkit.Views.Content.BaseContentForm
    {
        public SkillEditorForm(SkillTemplate template)
        {
            InitializeComponent();
            SetContentTemplate(template);
        }

        protected override void Save()
        {
            using (var db = new GameDatabaseContext())
            {
                var ContentTemplate = this.ContentTemplate as SkillTemplate;
                var repository = new SkillRepository(db);
                repository.Update(ContentTemplate, ContentTemplate.Id);
            }      
            base.Save();
        }



    }
}

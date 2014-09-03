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
using Server.Game.Database.Models.Quests;

namespace OpenORPG.Toolkit.Views.Content
{
    public partial class QuestEditorForm : OpenORPG.Toolkit.Views.Content.BaseContentForm
    {
        public QuestEditorForm(QuestTemplate questTemplate)
        {           
            InitializeComponent();
            SetContentTemplate(questTemplate);
        
        }

        protected override void Save()
        {
            using (var db = new GameDatabaseContext())
            {
                var ContentTemplate = this.ContentTemplate as QuestTemplate;
                var repository = new QuestRepository(db);
                repository.Update(ContentTemplate, ContentTemplate.Id);
            }      

            base.Save();
        }
    }
}

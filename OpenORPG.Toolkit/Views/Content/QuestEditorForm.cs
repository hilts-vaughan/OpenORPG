using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Windows.Forms;
using OpenORPG.Database.DAL;
using OpenORPG.Database.Models.Quests.Rewards;
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
            questRewardEditor1.Template = questTemplate;
        }


        protected override void Save()
        {
            var ContentTemplate = this.ContentTemplate as QuestTemplate;

            // Persist rewards
            ContentTemplate.Rewards = questRewardEditor1.Rewards;

            using (var db = new GameDatabaseContext())
            {

                var repository = new QuestRepository(db);
                repository.Update(ContentTemplate, ContentTemplate.Id);
            }

            base.Save();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Windows.Forms;
using OpenORPG.Database.DAL;
using OpenORPG.Database.Models.ContentTemplates;
using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Database.Models.Quests;

namespace OpenORPG.Toolkit.Views.Content
{
    public partial class NpcEditorForm : OpenORPG.Toolkit.Views.Content.BaseContentForm
    {
        protected NpcTemplate CurrentNpcTemplate
        {
            get { return (NpcTemplate)ContentTemplate; }
        }

        public NpcEditorForm(NpcTemplate npcTemplate)
        {
            InitializeComponent();
            SetContentTemplate(npcTemplate);


            textName.DataBindings.Add("Text", ContentTemplate, "Name");

            if (CurrentNpcTemplate.ConversationAvailableTemplate != null)
                textDialog.DataBindings.Add("Text", CurrentNpcTemplate.ConversationAvailableTemplate, "Name");

        }

        public NpcEditorForm()
        {
            InitializeComponent();
        }

        protected override void Save()
        {

            // (:: It might be a good idea to move some of the NPC update logic into the repo, but for now this works)
            using (var db = new GameDatabaseContext())
            {
                var ContentTemplate = this.ContentTemplate as NpcTemplate;

                var originalJob = db.Npcs.Include(j => j.ConversationAvailableTemplate).Include(x => x.Quests)
                    .Single(j => j.Id == ContentTemplate.Id);

                // Update scalar/complex properties
                db.Entry(originalJob).CurrentValues.SetValues(ContentTemplate);


                if (ContentTemplate.ConversationAvailableTemplate != null)
                {
                    var originalDialog =
                    db.DialogTemplates.Single(x => x.Id == ContentTemplate.ConversationAvailableTemplate.Id);
                    originalJob.ConversationAvailableTemplate = originalDialog;
                }

                if (ContentTemplate.Quests != null)
                {

                    List<QuestTemplate> templates = new List<QuestTemplate>();

                    foreach (var quest in ContentTemplate.Quests)
                    {
                        // For each quest we now have, try and find the new, opposing quest
                        var originalQuest = db.Quests.First(x => x.Id == quest.Id);
                        templates.Add(originalQuest);    
                    }

                    originalJob.Quests = templates;
                }



                db.SaveChanges();
            }

            base.Save();
        }

        private void buttonChooseDialog_Click(object sender, EventArgs e)
        {
            // We will need to prompt here
            var template = (DialogTemplate)PresentContentForm(typeof(DialogTemplate));

            if (template == null)
                return;

            // Assign the template as is
            CurrentNpcTemplate.ConversationAvailableTemplate = template;

            textDialog.DataBindings.Clear();
            textDialog.DataBindings.Add("Text", CurrentNpcTemplate.ConversationAvailableTemplate, "Name");
            textDialog.Update();
        }


        private IContentTemplate PresentContentForm(Type type)
        {
            var form = new ContentSelectionForm(type);
            form.ShowDialog();
            return form.SelectedTemplate;
        }



    }
}

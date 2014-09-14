using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenORPG.Database.DAL;
using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Database.Models.ContentTemplates;

namespace OpenORPG.Toolkit.Views.Content
{
    public partial class NpcEditorForm : OpenORPG.Toolkit.Views.Content.BaseContentForm
    {
        public NpcEditorForm(NpcTemplate npcTemplate)
        {
            InitializeComponent();
            SetContentTemplate(npcTemplate);

        }

        protected override void Save()
        {
            using (var db = new GameDatabaseContext())
            {
                var ContentTemplate = this.ContentTemplate as NpcTemplate;
                var repository = new NpcRepository(db);
                repository.Update(ContentTemplate, ContentTemplate.Id);
            }                   

            base.Save();
        }

     


    }
}

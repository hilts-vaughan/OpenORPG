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
    public partial class ItemEditorForm : OpenORPG.Toolkit.Views.Content.BaseContentForm
    {
        public ItemEditorForm(ItemTemplate itemTemplate)
        {
            InitializeComponent();
            SetContentTemplate(itemTemplate);

            propertyGrid1.SelectedObject = ContentTemplate;
        }

        protected override void Save()
        {
            using (var db = new GameDatabaseContext())
            {
                var ContentTemplate = this.ContentTemplate as ItemTemplate;
                var repository = new ItemRepository(db);
                repository.Update(ContentTemplate, ContentTemplate.Id);
            }      

            base.Save();
        }
    }
}

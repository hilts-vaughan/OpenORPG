using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenORPG.Database.DAL;
using OpenORPG.Database.Models.ContentTemplates;
using Server.Game.Database;
using Server.Infrastructure.Dialog;

namespace OpenORPG.Toolkit.Views.Content
{
    public partial class DialogEditor : BaseContentForm
    {

        /// <summary>
        /// The root of the tree being rendered for the dialog
        /// </summary>
        private DialogNode _rootDialogNode;

        private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public DialogEditor(DialogTemplate template)
        {
            InitializeComponent();
            SetContentTemplate(template);

            // Set reference
            _rootDialogNode = GetFromJson(template.JsonPayload);
        }

        private DialogNode GetFromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<DialogNode>(json, jsonSerializerSettings);
            }
            catch (Exception exception)
            {
                MessageBox.Show("The payload returned from the server was corrupt.");
                Close();
            }
        }

        private string ToJson()
        {
            return JsonConvert.SerializeObject(_rootDialogNode, jsonSerializerSettings);
        }


        protected override void Save()
        {
            var ContentTemplate = this.ContentTemplate as DialogTemplate;

            // Update the payload
            try
            {
                ContentTemplate.JsonPayload = ToJson();
            }

            catch (Exception exception)
            {
                MessageBox.Show("There was a problem saving the dialog. It was not saved. Aborting...");
                Close();
            }

            using (var db = new GameDatabaseContext())
            {

                var repository = new DialogRepository(db);
                repository.Update(ContentTemplate, ContentTemplate.Id);
            }

            base.Save();
        }



    }
}

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenORPG.Common.Dialog;
using OpenORPG.ContentProcessor.Persistence;
using OpenORPG.Database.DAL;
using Server.Game.Database;
using Server.Infrastructure.Dialog;
using ServiceStack;

namespace OpenORPG.ContentProcessor.Extractors
{
    public class DialogExtractor : IContentExtractor
    {
        private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto
        };

        public void ProcessContent(IContentPersister persister)
        {

            using (var db = new GameDatabaseContext())
            {
                var repo = new DialogRepository(db);
                var dialogs = repo.GetAll();

                foreach (var dialog in dialogs)
                {

                    // Save out properties we want to a new object and then persist
                    var dialogTemplate = JsonConvert.DeserializeObject<DialogNode>(dialog.JsonPayload, jsonSerializerSettings);

                    dynamic persistable = new ExpandoObject();

                    Console.WriteLine("Processing dialog with ID {0}", dialog.Id);

                    // TODO: Scrub conditions here so users cannot access them, as this would reveal information we do not desire

                    persistable = dialogTemplate;

                    persister.Persist(persistable, "\\dialogs\\{0}.json".FormatWith(dialog.Id));

                }

            }

        } //processContent


        /// <summary>
        /// Scrubs a dialog link of all the scripts and links associated with them using a deep recursive
        /// call to prevent leaking out information to the outside world about them.
        /// </summary>
        /// <param name="link"></param>
        private void ScrubLink(DialogLink link)
        {
            throw new NotImplementedException();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.ContentProcessor.Persistence;
using OpenORPG.Database.DAL;
using Server.Game.Database;
using ServiceStack;

namespace OpenORPG.ContentProcessor.Extractors
{
    /// <summary>
    /// An item extractor
    /// </summary>
    public class ItemExtractor : IContentExtractor
    {
        public void ProcessContent(IContentPersister persister)
        {
            using (var db = new GameDatabaseContext())
            {
                var repo = new ItemRepository(db);
                var items = repo.GetAll();

                foreach (var item in items)
                {

                    // Save out properties we want to a new object and then persist
                    dynamic persistable = new ExpandoObject();

                    Console.WriteLine("Processing item with ID {0}", item.Id);

                    persistable.id = item.Id;
                    persistable.name = item.Name;
                    persistable.type = item.Type;
                    persistable.equipSlot = item.EquipmentSlot;
                    persistable.description = item.Description;
                    persistable.restoreHp = item.RestoreHitpoints;

                    persister.Persist(persistable, "\\items\\{0}.json".FormatWith(item.Id));

                }

            }


        }
    }
}

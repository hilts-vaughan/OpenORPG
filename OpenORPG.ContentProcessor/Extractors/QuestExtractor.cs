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
    public class QuestExtractor : IContentExtractor
    {
        public void ProcessContent(IContentPersister persister)
        {


            using (var db = new GameDatabaseContext())
            {
                var repo = new QuestRepository(db);
                var items = repo.GetAll();

                foreach (var item in items)
                {

                    // Save out properties we want to a new object and then persist
                    dynamic persistable = new ExpandoObject();

                    Console.WriteLine("Processing quest with ID {0}", item.QuestTableId);

                    persistable.id = item.QuestTableId;
                    persistable.name = item.Name;
                    persistable.description = item.Description;



                    persister.Persist(persistable, "\\quests\\{0}.json".FormatWith(item.QuestTableId));

                }

            }



        }
    }
}

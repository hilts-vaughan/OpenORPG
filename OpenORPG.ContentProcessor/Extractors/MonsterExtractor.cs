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
    public class MonsterExtractor : IContentExtractor
    {
        public void ProcessContent(IContentPersister persister)
        {
            using (var db = new GameDatabaseContext())
            {
                var repo = new MonsterRepository(db);
                var monsters = repo.GetAll();

                foreach (var monster in monsters)
                {

                    // Save out properties we want to a new object and then persist
                    dynamic persistable = new ExpandoObject();

                    Console.WriteLine("Processing monster with ID {0}", monster.Id);

                    persistable.id = monster.Id;
                    persistable.name = monster.Name;
                    persistable.maxHp = monster.Hitpoints;
                    persistable.category = monster.VirtualCategory;
                    persistable.sprite = monster.Sprite;

                    persister.Persist(persistable, "\\monsters\\{0}.json".FormatWith(monster.Id));

                }

            }



        }
    }
}

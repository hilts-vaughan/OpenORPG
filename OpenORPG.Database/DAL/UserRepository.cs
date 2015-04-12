using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefactorThis.GraphDiff;
using Server.Game.Database;
using Server.Game.Database.Models;

namespace OpenORPG.Database.DAL
{
    public class UserRepository : DatabaseRepository<UserHero>
    {
        public UserRepository(GameDatabaseContext context) : base(context)
        {

        }

        protected override void PostUpdate(UserHero entity, int key)
        {
            // Allow for injection of graph updates
            Context.UpdateGraph(entity, map => map.OwnedCollection(p => p.Inventory));
            Context.UpdateGraph(entity, map => map.OwnedCollection(p => p.Skills));
            Context.UpdateGraph(entity, map => map.OwnedCollection(p => p.QuestInfo));
            Context.UpdateGraph(entity, map => map.OwnedCollection(p => p.Equipment));
            Context.UpdateGraph(entity, map => map.OwnedCollection(p => p.Flags));
            
            // Don't forget about quest requirements...
            foreach(var questInfo in entity.QuestInfo)
                Context.UpdateGraph(questInfo, map => map.OwnedCollection(p => p.RequirementProgress));

            base.PostUpdate(entity, key);
        }
    }
}

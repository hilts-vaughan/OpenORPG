using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefactorThis.GraphDiff;
using Server.Game.Database;
using Server.Game.Database.Models.Quests;

namespace OpenORPG.Database.DAL
{
    public class QuestRepository : DatabaseRepository<QuestTemplate>
    {
        public QuestRepository(GameDatabaseContext context)
            : base(context)
        {

        }

        protected override void PostUpdate(QuestTemplate entity, int key)
        {
            Context.UpdateGraph(entity, map => map.OwnedCollection(p => p.Rewards));
            Context.UpdateGraph(entity, map => map.OwnedCollection(p => p.QuestSteps));

            foreach (var step in entity.QuestSteps)
                Context.UpdateGraph(step, map => map.OwnedCollection(p => p.Requirements));

            base.PostUpdate(entity, key);
        }

        protected override void PostGet(List<QuestTemplate> entities)
        {

            foreach (var entity in entities)
            {
                Context.Entry(entity).Collection(a => a.Rewards).Load();
                Context.Entry(entity).Collection(a => a.QuestSteps)
                    .Query()
                    .Include(c => c.Requirements)
                    .Load();

            }

            base.PostGet(entities);
        }

    }
}

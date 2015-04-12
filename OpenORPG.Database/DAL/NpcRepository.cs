using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database;
using Server.Game.Database.Models;

namespace OpenORPG.Database.DAL
{
    public class NpcRepository : DatabaseRepository<NpcTemplate>
    {
        public NpcRepository(GameDatabaseContext context)
            : base(context)
        {
        }

        protected override void PostGet(List<NpcTemplate> entities)
        {

            // Load the dialog to be used
            foreach (var npc in entities)
            {
                Context.Entry(npc).Reference(a => a.ConversationAvailableTemplate).Load();    
            }


            base.PostGet(entities);
        }

        protected override void PostUpdate(NpcTemplate entity, int key)
        {
            // HACK: Always attempt to save
            Context.Entry(entity.ConversationAvailableTemplate).State = EntityState.Modified;

            base.PostUpdate(entity, key);
        }
    }
}

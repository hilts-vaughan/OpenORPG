using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Zones;
using Server.Infrastructure.World;
using Server.Infrastructure.World.Systems;

namespace Server.Game.AI
{
    public class AiSystem : GameSystem
    {
        public AiSystem(Zone world)
            : base(world)
        {

        }

        public override void Update(double frameTime)
        {

            // AI is costly; we won't bother updating if the zone is empty
            if (Zone.IsEmpty)
                return;

            foreach (var c in Zone.ZoneCharacters)
            {
                // Update AI
                if (c.CurrentAi != null)
                    c.CurrentAi.PerformUpdate(frameTime);
            }

        }

        public override void OnEntityAdded(Entity entity)
        {

        }

        public override void OnEntityRemoved(Entity entity)
        {
            // Remove agression for anyone who leaves
            foreach (var c in Zone.ZoneCharacters)
            {
                if (c.CurrentAi != null)
                    c.CurrentAi.AgressionTracker.RemoveAgression(entity.Id);
            }


        }
    }
}

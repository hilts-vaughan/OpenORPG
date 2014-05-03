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

        public override void Update(float frameTime)
        {
            foreach (var c in Zone.ZoneCharacters)
            {
                // Update AI
                if (c.CurrentAi != null)
                    c.CurrentAi.PerformUpdate(frameTime);
            }
        }

        public override void OnEntityAdded(Entity entity)
        {
            throw new NotImplementedException();
        }

        public override void OnEntityRemoved(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Zones;
using Server.Infrastructure.World;
using Server.Infrastructure.World.Systems;

namespace Server.Game.World.Systems
{
    /// <summary>
    /// A simple system that will monitor the world it belongs to and observe.
    /// </summary>
    public class DebugMonitorSystem : GameSystem 
    {

        public DebugMonitorSystem(Zone world) : base(world)
        {
            // 
        }

        public override void Update(double frameTime)
        {
            
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

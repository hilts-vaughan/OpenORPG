using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Game.Zones;
using Server.Infrastructure.World;
using Server.Infrastructure.World.Systems;

namespace Server.Game.Combat
{
    public class StatusEffectService : GameSystem
    {
        public StatusEffectService(Zone world)
            : base(world)
        {
        }

        public override void Update(double frameTime)
        {

        }

        public override void OnEntityAdded(Entity entity)
        {
            if (entity is Character)
                OnCharacterAdded(entity as Character);
        }


        public override void OnEntityRemoved(Entity entity)
        {
            if (entity is Character)
                OnCharacterRemoved(entity as Character);
        }

        private void OnCharacterAdded(Character character)
        {

        }

        private void OnCharacterRemoved(Character character)
        {

        }


    }
}

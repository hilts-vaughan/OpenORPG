using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Game.Zones;
using Server.Infrastructure.World;
using Server.Infrastructure.World.Systems;

namespace Server.Game.Combat
{
    public class LevelService: GameSystem
    {
        public LevelService(Zone world) : base(world)
        {
        }

        public override void Update(double frameTime)
        {

        }

        public override void OnEntityAdded(Entity entity)
        {
            if(entity is Player)
                OnPlayerAdded(entity as Player);

        }
       

        public override void OnEntityRemoved(Entity entity)
        {
            if(entity is Player)
                OnPlayerRemoved(entity as Player);
        }

        private void OnPlayerAdded(Player player)
        {
            player.ExperienceChanged += PlayerExperienceChanged;
            player.LevelChanged += PlayerOnLevelChanged;
        }      

        private void OnPlayerRemoved(Player player)
        {
            player.ExperienceChanged -= PlayerExperienceChanged;
            player.LevelChanged -= PlayerOnLevelChanged;
        }

        private void PlayerOnLevelChanged(int newValue, int oldValue, Player player)
        {
            // Do something if needed
        }

        void PlayerExperienceChanged(int newValue, int oldValue, Player player)
        {
            if(newValue >= player.Level * 500)
                player.PerformLevelUp();
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Infrastructure.Math;

namespace Server.Game.Zones.Spawns
{
    /// <summary>
    /// Defines a set of spawns that can spawn <see cref="Monster"/>s onto a zone.
    /// 
    /// </summary>
    public class MonsterSpawnSet : SpawnSet
    {

        /// <summary>
        /// Determines whether the spawned object is allowed to leave the bounds or not.
        /// </summary>
        public bool CanStray { get; set; }

        public long MobId { get; set; }

        public MonsterSpawnSet(int maximumAmount, bool repeat, float spawnTime, Rectangle spawnArea, bool canStray, long mobId) : base(maximumAmount, repeat, spawnTime, spawnArea)
        {
            CanStray = canStray;
            MobId = mobId;
            LastSpawnTime = spawnTime + 1;
        }

        public Monster TrySpawn(float time)
        {
            LastSpawnTime += time;

            if (LastSpawnTime > SpawnTime && SpawnedSoFar < MaximumAmount)
            {
                LastSpawnTime = 0;
                SpawnedSoFar++;
                return GameObjectFactory.CreateMonster(MobId);
            }

            return null;
        }
     

    }
}

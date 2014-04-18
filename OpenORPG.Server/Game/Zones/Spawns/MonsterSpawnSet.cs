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

        public Monster PerformCheck(float time)
        {
            LastSpawnTime += time;

            if (LastSpawnTime > SpawnTime)
            {
                LastSpawnTime = 0;
                return GameObjectFactory.CreateMonster(MobId);
            }

            return null;
        }

    }
}

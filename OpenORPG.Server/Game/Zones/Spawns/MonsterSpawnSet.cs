using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Game.Zones.Spawns
{
    /// <summary>
    /// Defines a set of spawns that can spawn <see cref="Npc"/>s onto a zone.
    /// 
    /// </summary>
    public class MonsterSpawnSet : SpawnSet
    {

        /// <summary>
        /// Determines whether the spawned object is allowed to leave the bounds or not.
        /// </summary>
        public bool CanStray { get; set; }

        public ulong MobId { get; set; }

        public MonsterSpawnSet(int maximumAmount, bool repeat, float spawnTime, bool canStray, ulong mobId) : base(maximumAmount, repeat, spawnTime)
        {
            CanStray = canStray;
            MobId = mobId;
        }

        public Npc PerformCheck(float time)
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

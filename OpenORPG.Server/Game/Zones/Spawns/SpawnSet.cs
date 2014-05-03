using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Math;
using Server.Infrastructure.World;

namespace Server.Game.Zones.Spawns
{
    /// <summary>
    /// A spawn set is responsible for emitting and managing spawns.
    /// <see cref="Entity"/>s can managed and spawned from these sets.
    /// </summary>
    public abstract class SpawnSet
    {
        // The last elapsed spawn time
        protected float LastSpawnTime = 0f;

        protected int SpawnedSoFar = 0;

        /// <summary>
        /// Defines the maximum amount of spawnable objects that can be created and managed by this set.
        /// This limits the amount of objects at any given time that can be spawned.
        /// </summary>
        public int MaximumAmount { get; set; }

        /// <summary>
        /// Defines whether or not this <see cref="SpawnSet"/> will repeat itself when complete.
        /// If false, no objects will be spawned again once <see cref="MaximumAmount"/> have been spawned.
        /// </summary>
        public bool Repeat { get; set; }

        /// <summary>
        /// Defines the amount of time required for a <see cref="SpawnSet"/> to spawn another object.
        /// This is measured in seconds. Fractional seconds are supported.
        /// </summary>
        public float SpawnTime { get; set; }

        public Rectangle SpawnArea { get; set; }


        protected SpawnSet(int maximumAmount, bool repeat, float spawnTime, Rectangle spawnArea)
        {
            MaximumAmount = maximumAmount;
            Repeat = repeat;
            SpawnTime = spawnTime;
            SpawnArea = spawnArea;
        }

        public void DecrementCounter()
        {
            SpawnedSoFar--;
        }

    }
}

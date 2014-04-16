using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Server.Infrastructure.Logging;
using Server.Infrastructure.World;
using Server.Infrastructure.World.Systems;

namespace Server.Game.Zones.Spawns
{
    /// <summary>
    /// Takes care of managing <see cref="SpawnSet"/>s for a particular <see cref="Zone"/>.
    /// This manager is in charge of determining when something is ready to spawn and creating,
    /// removing and cleaning up spawn sets.
    /// </summary>
    public class SpawnGameSystem : GameSystem 
    {
        private const string MobSpawnSetName = "MobSpawns";
        private const string MobSpawnSetType = "MobSpawn";

        private List<SpawnSet> _spawnSets = new List<SpawnSet>();

        public SpawnGameSystem(Zone zone) : base(zone)
        {
            // We will need to fetch and create our spawn sets from here
            CreateMonsterSpawns();
        }

        /// <summary>
        /// Creates <see cref="MonsterSpawnSet"/>s that are defined in the data provided
        /// by the <see cref="Zone"/> and adds them to the system to be interpreted.
        /// </summary>
        private void CreateMonsterSpawns()
        {
            var tileMap = Zone.TileMap;

            // If we have this layer, parse it
            if (tileMap.ObjectGroups.Contains(MobSpawnSetName))
            {
                var groups = tileMap.ObjectGroups[MobSpawnSetName];

                foreach (var mobSpawn in groups.Objects)
                {
                    if (mobSpawn.Type == MobSpawnSetType)
                    {
                        var mobId = Convert.ToUInt64(mobSpawn.Properties["MobId"]);
                        var mobCanStray = Convert.ToBoolean(mobSpawn.Properties["MobStrays"]);
                        var mobMaxAmount = Convert.ToInt32(mobSpawn.Properties["MobMax"]);
                        var mobSpawnTime = Convert.ToSingle(mobSpawn.Properties["MobSpawnTime"]);
                        var mobRepeats = Convert.ToBoolean(mobSpawn.Properties["MobRepeats"]);

                        var spawnSet = new MonsterSpawnSet(mobMaxAmount, mobRepeats, mobSpawnTime, mobCanStray, mobId);
                        _spawnSets.Add(spawnSet);
                    }
                    else
                    {
                        string error =
                            "Zone #{0} has an object incorrectly located on the {1} layer with an object of type {2}. ";
                        Logger.Instance.Info(error, Zone.Id, MobSpawnSetName, mobSpawn.Type);
                    }
                }
            }                   
        }

        public override void Update(float frameTime)
        {
            foreach (MonsterSpawnSet monsterSpawn in _spawnSets)
            {
                var npc = monsterSpawn.PerformCheck(frameTime);
                
                if(npc != null)
                    Zone.AddEntity(npc);
            }
        }

        public override void OnEntityAdded(Entity entity)
        {
            
        }

        public override void OnEntityRemoved(Entity entity)
        {
            var id = entity.Id;
        }



    }
}

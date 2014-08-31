using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using OpenORPG.Database.Models.Quests;
using Server.Game.Entities;
using Server.Game.Zones;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Quests.Requirements;
using Server.Infrastructure.World;

namespace Server.Infrastructure.Quests.Trackers
{
    /// <summary>
    /// A class that is instanced to keep track of various monster killing events for quest book keeping purposes. 
    /// </summary>
    public class MonsterKillCountQuestRequirementTracker : QuestRequirementTracker<QuestMonsterRequirementTable>
    {
        public MonsterKillCountQuestRequirementTracker(Zone zone)
            : base(zone)
        {

        }

        public override void OnEntityAdded(Entity entity)
        {
            if (entity is Monster)
                OnMonsterAdded(entity as Monster);
        }

        public override void OnEntityRemoved(Entity entity)
        {
            if (entity is Monster)
                OnMonsterRemoved(entity as Monster);
        }


        private void OnMonsterAdded(Monster monster)
        {
            monster.Killed += MonsterOnKilled;
        }

        private void OnMonsterRemoved(Monster monster)
        {
            monster.Killed -= MonsterOnKilled;
        }


        private void MonsterOnKilled(Character aggressor, Character victim)
        {
            // if we don't know who killed this monster, it's not important
            if (aggressor == null)
                return;

            var killerId = aggressor.Id;
            var monster = victim as Monster;
            var player = aggressor as Player;

            if (player == null)
                return;

            foreach (var activeRequirement in GetQuestEntryWithRequirementType<QuestMonstersKilledRequirement>(player))
            {
                // Fetch our results from our tuple
                var entry = activeRequirement.Item1;
                var requirement = activeRequirement.Item2;
                var i = activeRequirement.Item3;

                if (requirement.RequirementInfo.MonsterId == monster.MonsterTemplateId)
                {
                    var newValue = entry.IncrementProgress(i, 1);
                    OnProgressChanged(player, entry, i, newValue);
                    Logger.Instance.Info("Incrementing kill counter");
                }
            }

        }

    }
}

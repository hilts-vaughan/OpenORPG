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

            foreach (var entry in player.QuestLog.GetActiveQuestLogEntries())
            {
                var step = entry.CurrentStep;

                if (step != null)
                {
                    for (int i = 0; i < step.Requirements.Count; i++)
                    {
                        var requirement = step.Requirements[i];
                        var monsterReq = requirement as QuestMonstersKilledRequirement;

                        if (monsterReq != null)
                        {
                            if (monsterReq.RequirementInfo.MonsterId == monster.MonsterTemplateId)
                            {
                                var newValue = entry.IncrementProgress(i, 1);
                                OnProgressChanged(player, entry, i, newValue);
                                Logger.Instance.Info("Incrementing kill counter");
                            }
                        }

                    }

                }

            }


        }




    }
}

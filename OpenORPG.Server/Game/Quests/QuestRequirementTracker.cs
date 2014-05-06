using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models;
using Server.Game.Entities;
using Server.Infrastructure.Logging;

namespace Server.Game.Quests
{

    /// <summary>
    /// A class that is used to track and manage <see cref="Monster"/> kills
    /// and other requirements in the context of a quest. Information is typically
    /// funneled down by the zone object itself to feed out the information.
    /// </summary>
    public class QuestRequirementTracker
    {
        private Dictionary<long, List<MonsterKillInfo>> _requirementInfo = new Dictionary<long, List<MonsterKillInfo>>();

        public QuestRequirementTracker()
        {

        }

        public void LoadPlayer(Player player)
        {
            // Start our list
            _requirementInfo.Add((long)player.Id, new List<MonsterKillInfo>());


            foreach (var questInfo in player.QuestInfo)
            {
                MonitorQuest(player, questInfo);
            }
        }

        private void MonitorQuest(Player player, UserQuestInfo questInfo)
        {
            var list = _requirementInfo[(long)player.Id];
            var quest = QuestManager.Instance.GetQuest(questInfo.QuestId);
            var info = new MonsterKillInfo(quest.QuestTableId, quest.EndMonsterRequirements.MonsterId,
                quest.EndMonsterRequirements.MonsterAmount);
            list.Add(info);
        }

        public void UnloadPlayer(Player player)
        {
            _requirementInfo.Remove((long)player.Id);
        }

        public void OnMonsterAdded(Monster character)
        {
            character.Killed += CharacterOnKilled;
        }

        public void OnMonsterRemoved(Monster character)
        {
            character.Killed -= CharacterOnKilled;
        }

        private void CharacterOnKilled(Character aggressor, Character victim)
        {
            var killerId = aggressor.Id;
            var monster = victim as Monster;
            var player = aggressor as Player;

            if (_requirementInfo.ContainsKey((long)killerId))
            {
                foreach (var killInfo in _requirementInfo[(long)killerId])
                {
                    // If we know they match, find the according quest in the user
                    if (killInfo.MonsterId == monster.MonsterTemplateId)
                    {
                        foreach (var questInfo in player.QuestInfo)
                        {
                            if (questInfo.QuestId == killInfo.QuestId)
                            {
                                questInfo.MobsKilled++;

                                if (questInfo.MobsKilled > killInfo.MonsterAmount)
                                    questInfo.MobsKilled = killInfo.MonsterAmount;

                                Logger.Instance.Debug("{0} has killed an objective mob. Total count: {1}", player.Name, questInfo.MobsKilled);

                            }
                        }

                    }
                }

            }


        }


        public void NotifyBeginTracking(UserQuestInfo userQuestInfo, Player player)
        {
            MonitorQuest(player, userQuestInfo);
        }

    }


    public class MonsterKillInfo
    {
        public MonsterKillInfo(long questId, long monsterId, long monsterAmount)
        {
            QuestId = questId;
            MonsterId = monsterId;
            MonsterAmount = monsterAmount;
        }

        public long QuestId { get; set; }
        public long MonsterId { get; set; }
        public long MonsterAmount { get; set; }

    }


}
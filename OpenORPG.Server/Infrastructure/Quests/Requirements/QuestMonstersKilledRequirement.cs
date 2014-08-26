using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Models.Quests;
using Server.Game.Database.Models.Quests;
using Server.Game.Entities;

namespace Server.Infrastructure.Quests.Requirements
{
    public class QuestMonstersKilledRequirement : IQuestRequirement<QuestMonsterRequirementTable>, IQuestRequirement
    {
        public QuestMonstersKilledRequirement(QuestMonsterRequirementTable requirementInfo)
        {
            RequirementInfo = requirementInfo;
        }

        public QuestMonsterRequirementTable RequirementInfo { get; set; }

        public bool HasRequirements(Player player)
        {
            var quest = player.QuestInfo.First(x => x.QuestId == RequirementInfo.QuestStep.Quest.QuestTemplateId);
            return quest.MobsKilled == RequirementInfo.MonsterAmount;
        }

        public void TakeRequirements(Player player)
        {
            // Reset the mob count in case this quest can be repeated
            var quest = player.QuestInfo.First(x => x.QuestId == RequirementInfo.QuestStep.Quest.QuestTemplateId);
            quest.MobsKilled = 0;
        }

    }
}

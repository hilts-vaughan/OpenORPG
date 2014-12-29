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
    public class QuestMonstersKilledRequirement : QuestRequirement<QuestMonsterRequirementTable>, IQuestRequirement
    {
        public QuestMonstersKilledRequirement(QuestMonsterRequirementTable requirementInfo)
        {
            RequirementInfo = requirementInfo;

        }

      
        public QuestMonsterRequirementTable RequirementInfo { get; set; }

        //HACK: Used to add this to the client packet data
        public QuestMonsterRequirementTable Info { get { return RequirementInfo; } }

        public override QuestProgress GetQuestProgress(Player player)
        {
            return new QuestProgress(0, RequirementInfo.MonsterAmount);
        }

        public bool HasRequirements(Player player, int progress)
        {
            return progress >= RequirementInfo.MonsterAmount;
        }

        public void TakeRequirements(Player player)
        {
          // Just a simple monster counter, there's no need to reset this
        }

    }
}

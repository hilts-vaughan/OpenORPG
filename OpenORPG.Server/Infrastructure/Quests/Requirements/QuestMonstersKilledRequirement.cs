using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }

        public void TakeRequirements(Player player)
        {
            throw new NotImplementedException();
        }
    }
}

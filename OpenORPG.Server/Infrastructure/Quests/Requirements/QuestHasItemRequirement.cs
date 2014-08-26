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
    /// <summary>
    /// A concrete quest requirement that implements a check for having an item
    /// </summary>
    public class QuestHasItemRequirement : IQuestRequirement<QuestItemRequirementTable>, IQuestRequirement
    {
        public QuestHasItemRequirement(QuestItemRequirementTable table)
        {
            RequirementInfo = table;
        }

        public QuestItemRequirementTable RequirementInfo { get; set; }
        public QuestProgress GeQuestProgress(Player player)
        {
            //TODO Implement this progress amount properly
            return new QuestProgress(0, 0);
        }


        public bool HasRequirements(Player player)
        {
            return true;
        }

        public void TakeRequirements(Player player)
        {

        }


    }
}

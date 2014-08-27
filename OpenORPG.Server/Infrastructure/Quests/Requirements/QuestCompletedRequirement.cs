using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using Server.Game.Entities;

namespace Server.Infrastructure.Quests.Requirements
{
    /// <summary>
    /// A concrete quest requirement which requires the completion of another quest.
    /// </summary>
    public class QuestCompletedRequirement : IQuestRequirement
    {

        public int QuestId { get; set; }

        public QuestCompletedRequirement(int questId)
        {
            QuestId = questId;
        }

        public bool HasRequirements(Player player, int progress)
        {
            var quest = player.QuestLog.FirstOrDefault(x => x.Quest.QuestId == QuestId);

            if (quest != null)
                return quest.State == QuestState.Finished;

            return false;
        }

        public void TakeRequirements(Player player)
        {
            // There's no requirements to take here
        }
    }
}

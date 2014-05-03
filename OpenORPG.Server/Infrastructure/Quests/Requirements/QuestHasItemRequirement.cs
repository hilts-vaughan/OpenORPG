using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Infrastructure.Quests.Requirements
{
    /// <summary>
    /// A concrete quest requirement.
    /// </summary>
    public class QuestHasItemRequirement : IQuestRequirement
    {
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

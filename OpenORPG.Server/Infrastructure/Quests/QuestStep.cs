using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Infrastructure.Quests.Requirements;

namespace Server.Infrastructure.Quests
{
    /// <summary>
    /// A quest step is a step in the sequence of a quest in which a player must complete to obtain a reward and satasify requirements.
    /// </summary>
    public class QuestStep
    {

        /// <summary>
        /// These are requirements that must be met in order to 
        /// </summary>
        public List<IQuestRequirement> Requirements { get; set; }


        public bool IsRequirementsMet(Player player)
        {
            bool requirementsMet = true;

            foreach (var requirement in Requirements)
                requirementsMet &= requirement.HasRequirements(player);

            return requirementsMet;
        }

        public void TakeRequirements(Player player)
        {
            foreach(var requirement in Requirements)
                requirement.TakeRequirements(player);

        }

    }
}

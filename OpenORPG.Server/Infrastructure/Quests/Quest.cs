using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Quests.Requirements;
using Server.Infrastructure.Quests.Rewards;

namespace Server.Infrastructure.Quests
{
    /// <summary>
    /// A class provides an objective to a player 
    /// </summary>
    public class Quest
    {
        /// <summary>
        /// A list of starting requirements before this quest can be offered
        /// </summary>
        public List<IQuestRequirement> StartRequirements { get; private set; }

        /// <summary>
        /// A list of requirements to finish this quest
        /// </summary>
        public List<IQuestRequirement> FinishRequirements { get; private set; }

        /// <summary>
        /// A list of rewards this quest may give upon completion. 
        /// </summary>
        public List<IQuestReward> QuestRewards { get; private set; } 

        public ulong QuestId { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public bool CanRepeat { get; private set; }

        public Quest(ulong id)
        {
            // Create a quest
        }


    }
}

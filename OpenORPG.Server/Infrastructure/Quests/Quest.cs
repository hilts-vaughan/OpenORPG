using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models.Quests;
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

        public long QuestId { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public bool CanRepeat { get; private set; }


        /// <summary>
        /// Creates an instance of a quest given the accordingly table containing the information.
        /// </summary>
        /// <param name="questTable"></param>
        public Quest(QuestTable questTable)
        {
            Name = questTable.Name;
            Description = questTable.Description;
            QuestId = questTable.QuestTableId;
            CanRepeat = questTable.CanRepeat;

            // Load up requirements
            LoadStartRequirements(questTable);
            LoadFinishRequirements(questTable);

            // TODO: Give some logic for rewards here?
        }

        private void LoadStartRequirements(QuestTable questTable)
        {
            StartRequirements = new List<IQuestRequirement>();
        }

        private void LoadFinishRequirements(QuestTable questTable)
        {
            FinishRequirements = new List<IQuestRequirement>();

            // If we have some monster requirements
            if (questTable.EndMonsterRequirements != null)
            {
                var monsterRequirements = new QuestMonstersKilledRequirement(questTable.EndMonsterRequirements);
                FinishRequirements.Add(monsterRequirements);
            }

            // Check for item requirements
            if (questTable.EndItemRequirements != null)
            {
                var itemRequirements = new QuestHasItemRequirement(questTable.EndItemRequirements);
                FinishRequirements.Add(itemRequirements);
            }


        }


    }
}

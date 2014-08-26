using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using Server.Game.Database.Models.Quests;
using Server.Game.Entities;
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
        /// A list of steps that a user must take part in in order to finish a quest.
        /// </summary>
        public List<QuestStep> Steps { get; private set; }

        /// <summary>
        /// A list of starting requirements before this quest can be offered to a particular user.
        /// </summary>
        public List<IQuestRequirement> StartRequirements { get; private set; }


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
        public Quest(QuestTemplate questTable)
        {
            Name = questTable.Name;
            Description = questTable.Description;
            QuestId = questTable.QuestTemplateId;
            CanRepeat = questTable.CanRepeat;

            // Load up requirements
            LoadStartRequirements(questTable);
            LoadRewards(questTable);

        }

        private void LoadRewards(QuestTemplate questTable)
        {
            QuestRewards = new List<IQuestReward>();
            
            // Load from the template

        }

        /// <summary>
        /// Attempts to complete the quest and returns whether
        /// or not the attempt was successful 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool TryCompleteQuest(Player player)
        {
            // Verify the player actually has this quest
            var questInfo = player.QuestInfo.FirstOrDefault(x => x.QuestId == QuestId);

            if (questInfo == null)
                return false;

            bool requirementsMet = AreAllRequirementsMet(player);

            // Fail fast if requirements are not met
            if (!requirementsMet)
                return false;


            // Give the rewards as necessary
            bool canGive = TryGiveRewards(player);

            if (!canGive)
                return false;


            // Mark this quest as complete
            questInfo.State = QuestState.Finished;


            return true;

        }

        private bool TryGiveRewards(Player player)
        {
            bool validate = true;

            foreach (var reward in QuestRewards)
                validate = validate & reward.CanGive(player);

            // If we can't give something away for some reason...
            if (!validate)
                return false;

            foreach (var reward in QuestRewards)
                reward.Give(player);


            return true;
        }



        /// <summary>
        /// Runs through all the requirements and verifies whether or not
        /// they have all been met.
        /// </summary>
        /// <param name="player">The player to compare all the requirements against</param>
        /// <returns></returns>
        private bool AreAllRequirementsMet(Player player)
        {
            bool validated = true;

            foreach (var step in Steps)
                validated &= step.IsRequirementsMet(player);

            return validated;
        }


        private void LoadStartRequirements(QuestTemplate questTable)
        {
            StartRequirements = new List<IQuestRequirement>();
        }



    }
}

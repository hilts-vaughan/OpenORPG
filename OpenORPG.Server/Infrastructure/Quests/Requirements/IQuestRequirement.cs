﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using OpenORPG.Database.Models.Quests;
using Server.Game.Entities;
using Server.Game.Quests;

namespace Server.Infrastructure.Quests.Requirements
{
    /// <summary>
    /// A quest requirement interface that specifices a concrete implementation for a requirement   
    /// </summary>
    public abstract class QuestRequirement<T> where T : QuestRequirement
    {
 

        /// <summary>
        /// A quest requirement table containing information regarding what will be required to check
        /// this requirement properly.
        /// </summary>
        T RequirementInfo { get; set; }

        public string Type
        {
            get { return GetType().Name; }
        }


        /// <summary>
        /// Evaluates given the current context the current progress of a particular objective.
        /// </summary>
        /// <returns></returns>
        public abstract QuestProgress GetQuestProgress(Player player);
       

    }

    public interface IQuestRequirement
    {
        /// <summary>
        /// This method will do checks on the given player context and verify the requirements have been met
        /// to finish the quest successfully. 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        bool HasRequirements(Player player, int progress);

        /// <summary>
        /// This method is called directly after a succesful call of HasRequirements returning true.
        /// This is to take requirements that might be required right before ending a quest, such as
        /// turning in an item.
        /// </summary>
        /// <param name="player"></param>
        void TakeRequirements(Player player);
    }

}

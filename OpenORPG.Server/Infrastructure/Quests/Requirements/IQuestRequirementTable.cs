using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models.Quests;

namespace Server.Infrastructure.Quests.Requirements
{
    public interface IQuestRequirementTable
    {

        /// <summary>
        /// The quest Id that this entry represents a requirement for.
        /// </summary>
        long QuestTableId { get; set; }

    }
}

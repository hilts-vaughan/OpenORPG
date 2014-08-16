using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;

namespace Server.Game.Database.Models.Quests
{
    /// <summary>
    /// A quest requirement that involves killing a specific amount of monsters with the given <see cref="MonsterId"/>.
    /// </summary> 
    public class QuestMonsterRequirementTable : IQuestRequirementTable
    {
        public long QuestTableId { get; set; }

        /// <summary>
        /// The ID of the monster that must be slain to complete this requirement. 
        /// </summary>
        public long MonsterId { get; set; }

        /// <summary>
        /// The amount of the <see cref="MonsterId"/> that must be slain in order to complete this requirement.
        /// </summary>
        public long MonsterAmount { get; set; }

        public QuestTemplate Quest { get; set; }

    }
}

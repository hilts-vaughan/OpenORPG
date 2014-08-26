using System.ComponentModel.DataAnnotations.Schema;

namespace OpenORPG.Database.Models.Quests
{
    /// <summary>
    /// A quest requirement that involves killing a specific amount of monsters with the given <see cref="MonsterId"/>.
    /// </summary> 
       [Table("quest_requirements_monster")]
    public class QuestMonsterRequirementTable : QuestRequirement
    {

        /// <summary>
        /// The ID of the monster that must be slain to complete this requirement. 
        /// </summary>
        public long MonsterId { get; set; }

        /// <summary>
        /// The amount of the <see cref="MonsterId"/> that must be slain in order to complete this requirement.
        /// </summary>
        public long MonsterAmount { get; set; }

    }
}

using System.Collections.Generic;
using Server.Game.Entities;

namespace Server.Game.Combat.Actions
{
    /// <summary>
    /// Combat actions are the basis of interacting with other <see cref="Character"/>s in combat.
    /// Every action that is taken against another <see cref="Character"/> is a skill.
    /// 
    /// This includes attacking, healing, magic and items that are used.
    /// </summary>
    public interface ICombatAction
    {
        /// <summary>
        /// Executes the action in the context of all possible combat characters.
        /// </summary>
        void PerformAction(IEnumerable<Character> combatCharacters);

        /// <summary>
        /// The character that will be executing the action in the context.
        /// </summary>
        Character ExecutingCharacter{ get; set; }
    }
}

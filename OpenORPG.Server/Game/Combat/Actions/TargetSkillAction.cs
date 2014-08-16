using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Infrastructure.Logging;

namespace Server.Game.Combat.Actions
{
    /// <summary>
    /// Performs logic for a skill that requires a target. This is used for most ranged spells.
    /// </summary>
    public class TargetSkillAction : ICombatAction
    {
        public TargetSkillAction(Character executingCharacter, Skill skill)
        {
            ExecutingCharacter = executingCharacter;
            Skill = skill;
        }

        public CombatActionResult PerformAction(IEnumerable<Character> combatCharacters)
        {
            // Find our target in the list
            var target = combatCharacters.FirstOrDefault(x => x.Id == ExecutingCharacter.TargetId);

            if (target != null)
            {
                // This is the base damage computed as per the base engine; this does not take into account scripts
                // that may further impact things
                var damage = CombatUtility.ComputeDamage(ExecutingCharacter, target, Skill);

                //TODO: Come up with a better solution than this; this is a bit ugly
                target.CharacterStats[StatTypes.Hitpoints].CurrentValue -= damage;
            }

            // -1 means status failed
            Logger.Instance.Warn("{0} attempted to use a skill on a target but had no target.", ExecutingCharacter.Name);
            return new CombatActionResult(-1, 0);
        }

        public Character ExecutingCharacter { get; set; }
        public Skill Skill { get; set; }
        public float ExecutionTime { get; set; }
    }
}

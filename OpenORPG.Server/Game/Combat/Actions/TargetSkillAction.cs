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
    public class TargetSkillAction : CombatAction
    {
        public TargetSkillAction(Character executingCharacter, Skill skill, ulong targetId) : base(executingCharacter, skill)
        {
            TargetId = targetId;
        }

        public ulong TargetId { get; private set; }

        public override List<CombatActionResult> PerformAction(IEnumerable<Character> combatCharacters)
        {
            var targets = AcquireTargets(combatCharacters);
            var results = ExecuteSkill(targets);
            return results;
        }


        protected override IEnumerable<Character> AcquireTargets(IEnumerable<Character> combatCharacters)
        {
            // Find our target in the list
            var target = combatCharacters.Where(x => x.Id == TargetId).ToList();
            return target;
        }

    }
}

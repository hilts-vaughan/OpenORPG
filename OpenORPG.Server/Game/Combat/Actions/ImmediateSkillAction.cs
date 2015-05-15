using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Entities;
using Server.Utils.Math;

namespace Server.Game.Combat.Actions
{
    /// <summary>
    /// An immediate combat action is one which is performed directly within the immediate range of the user.
    /// A target is not required as activation is based solely on the activation.
    /// This is used for melee attacks and simple skills that execute around an area of a user.
    /// </summary>
    public class ImmediateSkillAction : CombatAction
    {
        public ImmediateSkillAction(Character executingCharacter, Skill skill)
            : base(executingCharacter, skill)
        {
        }


        public override List<CombatActionResult> PerformAction(IEnumerable<Character> combatCharacters)
        {

            // Grab a nearby target that is valid
            var targets = AcquireTargets(combatCharacters);

            // This prevents monsters from hitting each other and gaining aggro / player's from hitting others. 
            // In the future, some sort of 'rulebook' may need to be passed in to adjust the behaviour            
            targets = RemoveTargetsOfType(ExecutingCharacter.GetType(), targets);

            var results = ExecuteSkill(targets);
            return results;

        }

        protected override IEnumerable<Character> AcquireTargets(IEnumerable<Character> combatCharacters)
        {
            float highestDistance = float.MaxValue;
            Character target = null;

            foreach (var character in combatCharacters.Where(x => x.Id != ExecutingCharacter.Id))
            {
                var distance = Vector2.Distance(character.Position, ExecutingCharacter.Position);

                if (distance < highestDistance && distance < 70)
                {
                    highestDistance = distance;
                    target = character;
                }
            }
            return new List<Character>() { target };
        }

        private IEnumerable<Character> RemoveTargetsOfType(Type type, IEnumerable<Character> filterable)
        {
            return filterable.Where(x => x != null && x.GetType() != type);
        }

    }
}

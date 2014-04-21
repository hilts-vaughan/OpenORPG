using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class ImmediateSkillAction : ICombatAction
    {

        public ImmediateSkillAction(Character executingCharacter, Skill skill)
        {
            ExecutingCharacter = executingCharacter;
            Skill = skill;
        }



        public Character PerformAction(IEnumerable<Character> combatCharacters)
        {
            var validTargets = Skill.SkillTemplate.SkillTargetType;

            float highestDistance = float.MaxValue;
            Character target = null;

            foreach (var character in combatCharacters.Where(x => x.Id != ExecutingCharacter.Id))
            {
                var distance = Vector2.Distance(character.Position, ExecutingCharacter.Position);

                if (distance < highestDistance)
                {
                    highestDistance = distance;
                    target = character;
                }
            }

            if (target == null)
                return null;


            // Depending on the skill type, apply a payload or operation
            switch (Skill.SkillTemplate.SkillType)
            {
                case SkillType.Healing:
                    break;
                case SkillType.Damage:
                    var damagePayload = new DamagePayload(ExecutingCharacter);
                    damagePayload.Apply(target);
                    break;
                case SkillType.Special:
                    break;
            }

            return target;

        }

        public Character ExecutingCharacter { get; set; }
        public float ExecutionTime { get; set; }

        /// <summary>
        /// The skill template that is associated with this
        /// </summary>
        public Skill Skill { get; set; }
    }
}

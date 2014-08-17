using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Server.Game.Entities;

namespace Server.Game.Combat.Actions
{
    /// <summary>
    /// Combat actions are the basis of interacting with other <see cref="Character"/>s in combat.
    /// Every action that is taken against another <see cref="Character"/> is a skill.
    /// 
    /// This includes attacking, healing, magic and items that are used.
    /// </summary>
    public abstract class CombatAction
    {

        
        protected CombatAction(Character executingCharacter, Skill skill)
        {
            ExecutingCharacter = executingCharacter;
            Skill = skill;
        }

        /// <summary>
        /// Executes the action in the context of all possible combat characters.
        /// </summary>
        public abstract List<CombatActionResult> PerformAction(IEnumerable<Character> combatCharacters);

        protected List<CombatActionResult> ExecuteSkill(IEnumerable<Character> targets)
        {
            var results = new List<CombatActionResult>();

            //TODO: Implement some kind of fall off for AoE attacks so that damage isn't all the same
            foreach (var target in targets)
            {
                var damage = CombatUtility.ComputeDamage(ExecutingCharacter, target, Skill);
                var damagePayload = new DamagePayload(ExecutingCharacter, damage);
                target.ApplyDamage(damagePayload);

                var result = new CombatActionResult((long) target.Id, damage);
                results.Add(result);
            }

            return results;
        }


        /// <summary>
        /// Allows an action to specify a custom behaviour when 
        /// </summary>
        /// <param name="combatCharacters"></param>
        /// <returns></returns>
        protected abstract IEnumerable<Character> AcquireTargets(IEnumerable<Character> combatCharacters);

        /// <summary>
        /// The character that will be executing the action in the context.
        /// </summary>
        public Character ExecutingCharacter{ get; set; }

        public Skill Skill { get; set; }
            
        /// <summary>
        /// The amount of time remaining before an execution for this skill is possible
        /// </summary>
        public float ExecutionTime { get; set; }

    }
}

using Server.Game.Combat;
using Server.Game.Entities;

namespace Server.Infrastructure.Scripting.Combat
{
    /// <summary>
    /// Provides a way of extending the behavior of a basic status effect from the game core. 
    /// </summary>
    public class StatusEffectScript
    {

        /// <summary>
        /// This method is called when a status effects 'tick' has reached. 
        /// </summary>
        /// <param name="target"></param>
        public virtual void OnTick(Character target)
        {

        }

        /// <summary>
        /// This is called when a character afflicted by a status effect is attempting to perform damage.
        /// </summary>
        /// <param name="afflictedCharacter"></param>
        /// <param name="?"></param>
        /// <param name="skill"></param>
        /// <returns></returns>
        public virtual int OnCalculateSkillDamage(Character afflictedCharacter, Character target, Skill skill, int damage)
        {
            return damage;
        }

        /// <summary>
        /// Provides a method that is called when a user with this status effect is harmed via combat.
        /// </summary>
        /// <param name="afflictedCharacter"></param>
        /// <param name="agressor"></param>
        /// <param name="skill"></param>
        /// <param name="damage"></param>
        /// <returns></returns>
        public virtual int OnCalculateDamageTaken(Character afflictedCharacter, Character aggressor, Skill skill,
            int damage)
        {
            return damage;
        }



    }
}

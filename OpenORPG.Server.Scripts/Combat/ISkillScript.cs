using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Combat;
using Server.Game.Entities;

namespace OpenORPG.Server.Scripts.Combat
{
    /// <summary>
    /// This is an interface in which all skill scripts will optionally over-ride when
    /// </summary>
    public abstract class SkillScript
    {
        private readonly Skill _skill;

        protected SkillScript(Skill skill)
        {
            _skill = skill;
        }

        /// <summary>
        /// The skill that is attached to this
        /// </summary>
        public Skill Skill
        {
            get { return _skill; }
        }

        /// <summary>
        /// This method is called when damage is being computed for a skill. This method is 
        /// called immediately after the initial damage computation has been completed by the engine. 
        /// This will allows the skill to manipulate damage after the fact.
        /// 
        /// Use this for things like dealing double damage from time to time.
        /// </summary>
        /// <param name="attacker">This is the user of this skill, contains up to date information accordingly.</param>
        /// <param name="victim">This is the target of the skill. If this skill targets multiple users, it will be invoked once per target.</param>
        /// <returns></returns>
        public virtual int OnComputeDamage(Character attacker, Character victim, int damage)
        {
            return damage;
        }

        /// <summary>
        /// This method is called by the game engine core when a skill has been completed successfully.
        /// If you want to do something like kill a target instantly or apply conditional status effects,
        /// this is the method to use.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="victim"></param>
        public virtual void OnSkillFinished(Character attacker, Character victim)
        {

        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using Server.Game.Entities;

namespace Server.Game.Combat.Targeting
{
    /// <summary>
    /// A class designed to validate whether a given skill and target is valid
    /// </summary>
    public class TargetValidator
    {
        /// <summary>
        /// Given a target, user and skill; determines whether or not it's possible to use a skill on the target.
        /// </summary>
        /// <param name="skill">The skill that we want to test against</param>
        /// <param name="user">The user of the skill</param>
        /// <param name="target">The proposed target of the skill</param>
        /// <returns></returns>
        public bool IsTargetValid(Skill skill, Character user, Character target)
        {
            var flags = skill.SkillTemplate.SkillTargetType;
            var canTarget = true;

            // Next, we simply filter based on flags

            if (flags.HasFlag(SkillTargetType.Self))
                canTarget &= IsSelf(target, user);

            if (flags.HasFlag(SkillTargetType.Enemy))
                canTarget &= IsTargetEnemy(target, user);

            if (flags.HasFlag(SkillTargetType.Ally))
                canTarget &= IsAlly(target, user);



            // This is a special case, it's always last. It over-rides everything.
            if (flags.HasFlag(SkillTargetType.Anyone))
                canTarget = true;

            return canTarget;
        }

        private bool IsAlly(Character target, Character user)
        {
            //TODO: Implement some actual ally logic here when parties are implemented; for now it's assumed nobody is your ally since there are none
            return false;
        }

        private bool IsTargetEnemy(Character target, Character user)
        {
            return target is Monster;
        }

        private bool IsSelf(Character target, Character user)
        {
            return target.Id == user.Id;
        }


    }
}

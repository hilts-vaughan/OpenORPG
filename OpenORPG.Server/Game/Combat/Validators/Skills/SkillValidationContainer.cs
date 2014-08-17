using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Game.Combat.Validators.Skills
{
    /// <summary>
    /// This is used when creating skill validators to keep parameters clean.
    /// </summary>
    public class SkillValidationContainer
    {
        public Skill Skill { get; set; }

        public Character User { get; set; }

        public Character Target { get; set; }

        public SkillValidationContainer(Skill skill, Character user, Character target)
        {
            Skill = skill;
            User = user;
            Target = target;
        }
    }
}

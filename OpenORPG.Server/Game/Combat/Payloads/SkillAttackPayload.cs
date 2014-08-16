using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Game.Combat.Payloads
{
    class SkillAttackPayload : Payload
    {
        private Skill _skill;

        public SkillAttackPayload(Character aggressor, Skill skill) : base(aggressor)
        {
            _skill = skill;
        }

        public override void Apply(Character victim)
        {
        
        }

    }
}

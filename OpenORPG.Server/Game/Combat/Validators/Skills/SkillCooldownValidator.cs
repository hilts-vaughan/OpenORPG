using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Utils.SpecificationPattern;

namespace Server.Game.Combat.Validators.Skills
{
    public class SkillCooldownValidator : ISpecification<SkillValidationContainer>
    {
        public bool IsSatisfiedBy(SkillValidationContainer entity)
        {
            return entity.Skill.IsNotOnCooldown();
        }
    }
}

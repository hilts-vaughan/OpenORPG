using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Utils.SpecificationPattern;

namespace Server.Game.Combat.Validators.Skills
{
    /// <summary>
    /// A validator that ensures a character has the required resources to use a skill.
    /// </summary>
    public class SkillResourceCostValidator : ISpecification<SkillValidationContainer>
    {
        public bool IsSatisfiedBy(SkillValidationContainer entity)
        {
            return entity.User.CharacterStats[StatTypes.SkillResource].CurrentValue >=
                   entity.Skill.SkillTemplate.SkillCost;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Utils.Math;
using Server.Utils.SpecificationPattern;

namespace Server.Game.Combat.Validators.Skills
{
    /// <summary>
    ///  A validator that is used to check if a target is within range for a particular skill to be used.
    /// </summary>
    public class SkillTargetInRangeValidator : ISpecification<SkillValidationContainer>
    {
        public bool IsSatisfiedBy(SkillValidationContainer entity)
        {
            // A distance of 0 means you can cast from anywhere
            if (entity.Skill.SkillTemplate.Distance == 0)
                return true;

            return Vector2.Distance(entity.User.Position, entity.Target.Position) <= entity.Skill.SkillTemplate.Distance;

        }
    }
}

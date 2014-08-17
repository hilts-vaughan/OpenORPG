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
    /// Produces chained validations that can be evaluated, given a user, target and skill.
    /// This keeps requirements isolated well.
    /// </summary>
    public static class SkillValidationUtility
    {
        private static SkillCooldownValidator _skillCooldownValidator = new SkillCooldownValidator();
        private static SkillResourceCostValidator _skillResourceCostValidator = new SkillResourceCostValidator();
        private static SkillTargetInRangeValidator _skillTargetInRangeValidator = new SkillTargetInRangeValidator();
        private static TargetValidator _targetValidator = new TargetValidator();

        public static bool CanPerformSkill(Character user, Character target, Skill skill)
        {
            var container = new SkillValidationContainer(skill, user, target);
            var result = true;

            // Every skill needs to have these validators applied
            result &= EvaluateRequirementsForAllSkillTypes(container);

            // If your skill has a target, apply the below validators
            result &= EvaluateRequirementsForTargetSkillTypes(container);

            return result;
        }

        private static bool EvaluateRequirementsForTargetSkillTypes(SkillValidationContainer container)
        {
            return _skillTargetInRangeValidator.And(_targetValidator).IsSatisfiedBy(container);
        }

        private static bool EvaluateRequirementsForAllSkillTypes(SkillValidationContainer container)
        {
            return _skillCooldownValidator.And(_skillResourceCostValidator).IsSatisfiedBy(container);
        }



    }
}

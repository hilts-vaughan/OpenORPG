using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using Server.Game.Combat.Actions;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Entities;
using Server.Infrastructure.Logging;

namespace Server.Game.Combat
{
    /// <summary>
    /// Generates actions to be used in combat based off of state fed into it.
    /// </summary>
    public class ActionGenerator
    {


        /// <summary>
        /// Generates an immediate skill action.
        /// </summary>
        private ImmediateSkillAction CreateImmediateSkillAction(Skill skill, Character user)
        {
            return new ImmediateSkillAction(user, skill);
        }

        public ICombatAction GenerateActionFromSkill(Skill skill, long targetId, Character requestingHero)
        {
            ICombatAction action = null;
            var skillTemplate = skill.SkillTemplate;

            switch (skillTemplate.SkillActivationType)
            {
                case SkillActivationType.Immediate:
                    return CreateImmediateSkillAction(skill, requestingHero);
                case SkillActivationType.Target:
                    return CreateTargetSkillAction(skill, requestingHero);
                default:
                    Logger.Instance.Warn("{0} is not a supported skill type, invoked on skill #{1}",
                        skillTemplate.SkillType.ToString(), skillTemplate.Id);
                    break;
            }

            return action;
        }

        private ICombatAction CreateTargetSkillAction(Skill skill, Character requestingHero)
        {
            return new TargetSkillAction(requestingHero, skill);
        }
    }
}

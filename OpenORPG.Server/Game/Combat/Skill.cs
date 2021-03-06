﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Entities;
using Server.Infrastructure.Scripting;
using Server.Infrastructure.Scripting.Combat;

namespace Server.Game.Combat
{
    /// <summary>
    /// Represents a skill that a <see cref="Character"/> is able to use and perform.
    /// </summary>
    public class Skill
    {
        private SkillTemplate _skillTemplate;


        /// <summary>
        /// Returns the amount of time remaining on the cool down.
        /// 
        /// A value of '0' implies the skill is ready to use.
        /// </summary>
        public double Cooldown { get; set; }

        public long Id
        {
            get { return SkillTemplate.Id; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SkillScript Script { get; private set; }

        /// <summary>
        /// Returns the skill template in use by this skill
        /// </summary>
        [JsonIgnore]
        public SkillTemplate SkillTemplate
        {
            get { return _skillTemplate; }
        }

        public Skill(SkillTemplate skillTemplate)
        {
            _skillTemplate = skillTemplate;
            Script = ScriptLoader.Instance.GetSkillScript(this);
        }

        /// <summary>
        /// Sets the cooldown time to maximum amount possible
        /// </summary>
        public void ResetCooldown()
        {
            Cooldown = _skillTemplate.CooldownTime;
        }

        /// <summary>
        /// Determines whether or not the skill is usable
        /// </summary>
        /// <returns></returns>
        public bool IsNotOnCooldown()
        {
            return Cooldown <= 0;
        }




    }
}

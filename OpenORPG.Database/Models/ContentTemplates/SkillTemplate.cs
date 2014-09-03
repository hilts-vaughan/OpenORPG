
using System.ComponentModel.DataAnnotations.Schema;
using OpenORPG.Database.Enums;

namespace Server.Game.Database.Models.ContentTemplates
{
    /// <summary>
    ///     A skill template represents a given skill in the database
    /// </summary>
    [Table("skill_template")]
    public class SkillTemplate : IContentTemplate
    {
 

        public SkillTemplate(SkillType skillType, SkillTargetType skillTargetType, SkillActivationType skillActivationType, float castTime, long damage, string description, int id, string name)
        {
            SkillType = skillType;
            SkillTargetType = skillTargetType;
            SkillActivationType = skillActivationType;
            CastTime = castTime;
            Damage = damage;
            Description = description;
            Id = id;
            Name = name;


            CooldownTime = 1f;
        }

        public SkillTemplate()
        {
            
        }

        /// <summary>
        /// The icon index this skill will represent visually
        /// </summary>
        public int IconId { get; set; }

        public int AnimationId { get; set; }

        /// <summary>
        /// This is a numeric value of the cost to cast a specific spell. A character
        /// must have at least this amount of cost to be able to perform a given skill.
        /// </summary>
        public int SkillCost { get; set; }


        /// <summary>
        /// The script you want to attach to this skill
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        /// The distance this skill can target
        /// </summary>
        public int Distance { get; set; }

        public SkillType SkillType { get; set; }
        public SkillTargetType  SkillTargetType { get; set; }
        public SkillActivationType SkillActivationType { get; set; }

        public string Rawr { get; set; }

        public SkillCategory SkillCategory { get; set; }

        public SkillAttribute SkillAttribute{ get; set; }        

        

        /// <summary>
        /// The amount of time (seconds) it takes to perform this skill
        /// 
        /// A time of 0 indicates that this skill can be performed instantly.
        /// </summary>
        public float CastTime { get; set; }

        /// <summary>
        /// The amount of time this skill takes to reuse
        /// </summary>
        public float CooldownTime { get; set; }

        /// <summary>
        /// The amount of damage this skill performs
        /// </summary>
        public long Damage { get; set; }

        public string Description { get; set; }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string VirtualCategory { get; set; }
    
    
    }
}
using Server.Game.Combat;

namespace Server.Game.Database.Models.ContentTemplates
{
    /// <summary>
    ///     A skill template represents a given skill in the database
    /// </summary>
    public class SkillTemplate : IContentTemplate
    {
 

        public SkillTemplate(SkillType skillType, SkillTargetType skillTargetType, SkillActivationType skillActivationType, long castTime, long damage, string description, long id, string name)
        {
            SkillType = skillType;
            SkillTargetType = skillTargetType;
            SkillActivationType = skillActivationType;
            CastTime = castTime;
            Damage = damage;
            Description = description;
            Id = id;
            Name = name;
        }

        public SkillTemplate()
        {
            
        }

        public SkillType SkillType { get; set; }
        public SkillTargetType  SkillTargetType { get; set; }
        public SkillActivationType SkillActivationType { get; set; }


        /// <summary>
        /// The amount of time (milliseconds) it takes to perform this skill
        /// 
        /// A time of 0 indicates that this skill can be performed instantly.
        /// </summary>
        public long CastTime { get; set; }

        /// <summary>
        /// The amount of damage this skill performs
        /// </summary>
        public long Damage { get; set; }

        public string Description { get; set; }
        
        public long Id { get; set; }
        public string Name { get; set; }
        public string VirtualCategory { get; set; }
    
    
    }
}
using Server.Game.Combat;

namespace Server.Game.Database.Models.ContentTemplates
{
    /// <summary>
    ///     A skill template represents a given skill in the database
    /// </summary>
    public class SkillTemplate : IContentTemplate
    {
        public SkillTemplate(string description, long id, string name, string garbage)
        {
            Description = description;
            Id = id;
            Name = name;
            Garbage = garbage;
        }

        public SkillType SkillType { get; set; }
        public SkillTargetType  SkillTargetType { get; set; }
        public SkillActivationType SiSkillActivationType { get; set; }

        public string Description { get; set; }
        public string Garbage { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string VirtualCategory { get; set; }
    
    
    }
}
namespace Server.Game.Database.Models.ContentTemplates
{
    /// <summary>
    ///     A skill template represents a given skill in the database
    /// </summary>
    public class SkillTemplate : IContentTemplate
    {
        public SkillTemplate(string description, int id, string name, string garbage)
        {
            Description = description;
            Id = id;
            Name = name;
            Garbage = garbage;
        }

        public string Description { get; set; }
        public string Garbage { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string VirtualCategory { get; set; }
    }
}
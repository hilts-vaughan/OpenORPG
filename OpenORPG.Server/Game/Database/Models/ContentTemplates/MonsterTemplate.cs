namespace Server.Game.Database.Models.ContentTemplates
{
    public class MonsterTemplate : IContentTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VirtualCategory { get; set; }
    }
}
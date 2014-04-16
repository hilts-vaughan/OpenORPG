namespace Server.Game.Database.Models.ContentTemplates
{
    public class MonsterTemplate : IContentTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VirtualCategory { get; set; }

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Vitality { get; set; }
        public int Intelligence { get; set; }
        public int Level { get; set; }

        public int Hitpoints { get; set; }

    }
}
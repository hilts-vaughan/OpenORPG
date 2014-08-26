using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Game.Database.Models.ContentTemplates
{
    [Table("monster_template")]
    public class MonsterTemplate : IContentTemplate, IStatTemplate
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string VirtualCategory { get; set; }
        public string Sprite { get; set; }
    
        public int Level { get; set; }


        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Vitality { get; set; }
        public int Intelligence { get; set; }
        public int Hitpoints { get; set; }
        public int MaximumHitpoints { get; set; }
        public int SkillResource { get; set; }
        public int MaximumSkillResource { get; set; }
        public int Mind { get; set; }
        public int Luck { get; set; }
    }
}
using System.Data.Entity.ModelConfiguration;
using Server.Game.Database.Models;
using Server.Game.Database.Models.ContentTemplates;

namespace Server.Game.Database.Maps
{
    public class SkillTemplateMap : EntityTypeConfiguration<SkillTemplate>
    {
        public SkillTemplateMap()
        {
            // Primary Key
            HasKey(t => t.Id);


            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(45);

            Property(t => t.SkillType)
                .IsRequired();

            Property(t => t.SkillTargetType).IsRequired();
            Property(t => t.SiSkillActivationType).IsRequired();

        }
    }
}
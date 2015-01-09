using System.Data.Entity.ModelConfiguration;
using Server.Game.Database.Models;
using Server.Game.Database.Models.ContentTemplates;

namespace Server.Game.Database.Maps
{
    public class ItemTemplateMap : EntityTypeConfiguration<ItemTemplate>
    {
        public ItemTemplateMap()
        {
            // Primary Key
            HasKey(t => t.Id);


            // Properties
            Property(t => t.Name)
                .HasMaxLength(45);

        }
    }
}
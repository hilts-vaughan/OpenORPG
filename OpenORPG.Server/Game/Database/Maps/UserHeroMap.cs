using System.Data.Entity.ModelConfiguration;
using Server.Game.Database.Models;

namespace Server.Game.Database.Maps
{
    public class UserHeroMap : EntityTypeConfiguration<UserHero>
    {
        public UserHeroMap()
        {
            // Primary Key
            HasKey(t => t.Id);


            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(45);

            Property(t => t.Strength)
                .IsRequired();
            Property(t => t.Dexterity)
                .IsRequired();
            Property(t => t.Intelligence)
                .IsRequired();
            Property(t => t.Vitality)
                .IsRequired();
            Property(t => t.Hitpoints)
                .IsRequired();
            Property(t => t.Experience)
                .IsRequired();
            Property(t => t.Level)
                .IsRequired();
            Property(t => t.Gold)
                .IsRequired();
      
        

        
        }
    }
}
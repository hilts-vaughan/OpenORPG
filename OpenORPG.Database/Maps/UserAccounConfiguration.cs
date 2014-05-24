using System.Data.Entity.ModelConfiguration;
using Server.Game.Database.Models;

namespace Server.Game.Database.Maps
{

    public class UserAccounConfiguration : EntityTypeConfiguration<UserAccount>
    {
        public UserAccounConfiguration()
        {
            // Primary Key
            HasKey(t => t.AccountId);

            // Properties
            Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(45);

            // Properties
            Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(45);


            // Properties
            Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(45);

            Property(t => t.IsOnline)
                .IsRequired();

            HasMany(u => u.Heroes)
                .WithRequired(x => x.Account)
                .HasForeignKey(h => h.AccountId);
        }
    }
}
using System.Data.Entity.ModelConfiguration;
using Server.Game.Database.Models;

namespace Server.Game.Database.Maps
{

    public class UserStorageMap : EntityTypeConfiguration<UserItem>
    {


        public UserStorageMap()
        {
            HasKey(t => t.ItemEntryId);

       

        }

    }
}

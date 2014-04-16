using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models;
using Server.Game.Database.Models.ContentTemplates;

namespace Server.Game.Database.Maps
{

    public class UserStorageMap : EntityTypeConfiguration<UserStorage>
    {


        public UserStorageMap()
        {
            HasKey(t => t.UserId);

            Property(e => e.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(e => e.User).WithRequiredDependent(e => e.Inventory);

        }

    }
}

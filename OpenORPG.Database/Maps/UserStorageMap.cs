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


    public class UserSkillMap : EntityTypeConfiguration<UserSkill>
    {
        public UserSkillMap()
        {
            HasKey(t => t.UserSkillId);

        }
    }

    public class UserEquipmentMap : EntityTypeConfiguration<UserEquipment>
    {

        public UserEquipmentMap()
        {
            HasKey(t => t.UserEquipmentId);
        }

    }

}

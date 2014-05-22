using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Server.Game.Database.Models.ContentTemplates;
using Server.Infrastructure.Quests;

namespace Server.Game.Database.Models
{
    /// <summary>
    ///     A UserHero contains information pertaining to a particular hero; a <see cref="UserAccount" /> may contain several of these
    /// </summary>
    public class UserHero : IStatTemplate
    {
        public UserHero(UserAccount account, int positionX, int positionY, long id, string name)
        {
            Account = account;
            PositionX = positionX;
            PositionY = positionY;
            ZoneId = id;
            Name = name;

            CreationDate = DateTime.UtcNow;

            Inventory = new List<UserItem>();
            Skills = new List<UserSkill>();
            QuestInfo = new List<UserQuestInfo>();
            Equipment = new List<UserEquipment>();
        }

        public UserHero()
        {
            Inventory = new List<UserItem>();
            Skills = new List<UserSkill>();
            QuestInfo = new List<UserQuestInfo>();
            Equipment = new List<UserEquipment>();
        }

        public int UserHeroId { get; set; }
        public int AccountId { get; set; }

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Vitality { get; set; }
        public int Intelligence { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
        public int Hitpoints { get; set; }
        public int MaximumHitpoints { get; set; }
        public int Luck { get; set; }


        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public long ZoneId { get; set; }


        public long HomepointZoneId { get; set; }
        public long HomepointZoneX { get; set; }
        public long HomepointZoneY { get; set; }

        public virtual ICollection<UserItem> Inventory { get; set; }

        public virtual ICollection<UserEquipment> Equipment { get; set; } 

        public virtual ICollection<UserSkill> Skills { get; set; }

        public virtual ICollection<UserQuestInfo> QuestInfo { get; set; }

        public DateTime? CreationDate { get; set; }

        /// <summary>
        ///     A reference to the account this character is owned by
        /// </summary>
        public virtual UserAccount Account { get; set; }


        /// <summary>
        ///     The name of this character
        /// </summary>
        public string Name { get; set; }
    }
}
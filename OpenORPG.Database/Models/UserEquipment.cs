using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;

namespace Server.Game.Database.Models
{
    [Table("user_equipment")]
    public class UserEquipment
    {
        public int UserEquipmentId { get; set; }

        public long ItemId { get; set; }
        public EquipmentSlot Slot { get; set; }

        /// <summary>
        /// This refers to the user that this storage belongs to
        /// </summary>
        public virtual UserHero User { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;

namespace Server.Game.Database.Models
{
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

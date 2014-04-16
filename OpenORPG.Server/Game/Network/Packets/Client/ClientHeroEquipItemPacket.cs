using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Items.Equipment;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Client
{
    public struct ClientHeroEquipItemPacket : IPacket
    {
        /// <summary>
        /// The slot ID in the users inventory to equip from
        /// </summary>
        public long SlotId { get; set; }

        /// <summary>
        /// The desired equipment slot in the users equipment.
        /// </summary>
        public EquipmentSlot EquipmentSlot { get; set; }


        public ClientHeroEquipItemPacket(long slotId, EquipmentSlot equipmentSlot) : this()
        {
            SlotId = slotId;
            EquipmentSlot = equipmentSlot;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_HERO_EQUIP; }
        }

    }
}

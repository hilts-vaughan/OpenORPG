using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Items.Equipment;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{
    public struct ServerUnequipItemPacket : IPacket
    {
        public ServerUnequipItemPacket(EquipmentSlot slot) : this()
        {
            Slot = slot;
        }

        public EquipmentSlot Slot { get; set; }

        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_UNEQUIP_ITEM; }
        }


    }
}

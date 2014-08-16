using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using Server.Game.Items.Equipment;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{
    class ServerEquipmentUpdatePacket : IPacket
    {
        public ServerEquipmentUpdatePacket(Equipment equipment, EquipmentSlot slot)
        {
            Equipment = equipment;
            Slot = slot;
        }

        public OpCodes OpCode
        {
            get {  return OpCodes.SMSG_EQUIPMENT_UPDATE;}
        }

        public Equipment Equipment { get; set; }
        public EquipmentSlot Slot { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Items.Equipment;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Client
{
    public struct ClientHeroUseItemPacket : IPacket
    {
        /// <summary>
        /// The slot ID in the users inventory to equip from
        /// </summary>
        public long SlotId { get; set; }

        public ClientHeroUseItemPacket(long slotId) : this()
        {
            SlotId = slotId;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_ITEM_USE; }
        }

    }
}

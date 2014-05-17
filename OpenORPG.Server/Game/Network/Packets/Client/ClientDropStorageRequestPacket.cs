using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Client
{
    public class ClientDropStorageRequestPacket : IPacket
    {
        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_STORAGE_DROP; }
        }

        public long SlotId { get; set; }
        public long Amount { get; set; }

        public ClientDropStorageRequestPacket(long slotId, long amount)
        {
            SlotId = slotId;
            Amount = amount;
        }

    }
}

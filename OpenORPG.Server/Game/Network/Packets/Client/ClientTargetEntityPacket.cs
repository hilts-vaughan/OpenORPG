using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Client
{
    public struct ClientTargetEntityPacket : IPacket
    {
        public ClientTargetEntityPacket(ulong entityId) : this()
        {
            EntityId = entityId;
        }

        public ulong EntityId { get; set; }

        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_ENTITY_TARGET; }
        }
    }
}

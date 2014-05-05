using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Client
{
    /// <summary>
    /// This packet is sent by the client when an interaction is requested on the client side.
    /// This is typically used for interacting with NPCs
    /// </summary>
    public struct ClientRequestInteractionPacket : IPacket
    {
        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_INTERACT_REQUEST; }
        }
    }
}

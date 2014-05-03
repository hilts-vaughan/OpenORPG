using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{
    public struct ServerLeaveChatChannelPacket : IPacket
    {
        public int ChannelId { get; set; }

        public ServerLeaveChatChannelPacket(int channelId) : this()
        {
            ChannelId = channelId;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_LEAVE_CHAT_CHANNEL;  }
        }

    }
}

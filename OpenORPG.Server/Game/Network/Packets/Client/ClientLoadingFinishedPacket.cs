using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Client
{
    public struct ClientLoadingFinishedPacket : IPacket
    {
 

        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_GAME_LOADED; }
        }
    }
}

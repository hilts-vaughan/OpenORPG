using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Client
{

    /// <summary>
    /// This packet is sent when a client sends a request to teleport around a map using clicking means.
    /// This packet can also be sent for requests to warp that usually break the rules, too without issues.
    /// For example, if a client wants to move half way across the map.
    /// 
    /// This usually requires special permissions to do so. 
    /// </summary>
    public struct ClientClickWarpRequest : IPacket
    {
        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_CLICK_WARP_REQUEST; }
        }

        public int X { get; set; }
        public int Y { get; set; }

        public ClientClickWarpRequest(int y, int x) : this()
        {
            Y = y;
            X = x;
        }

    }
}

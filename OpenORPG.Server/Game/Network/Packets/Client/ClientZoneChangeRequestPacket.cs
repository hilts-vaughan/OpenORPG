using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Client
{
    public class ClientZoneChangeRequestPacket : IPacket
    {
        public OpCodes OpCode
        {
            get
            {
                return OpCodes.CMMSG_ZONE_CHANGE;
            }
        }

        /// <summary>
        /// The ZoneId the client has requested to change to
        /// </summary>
        public Direction Direction{ get; set; }


        public ClientZoneChangeRequestPacket(Direction direction)
        {
            Direction = direction;
        }
    }
}

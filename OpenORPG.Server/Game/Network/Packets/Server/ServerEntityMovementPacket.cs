using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;
using Server.Utils.Math;

namespace Server.Game.Network.Packets.Server
{
    public class ServerEntityMovementPacket : IPacket
    {
        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_ENTITY_MOVE; }
        }

        public Vector2 Position { get; set; }
        public Direction Direction { get; set; }

        public ServerEntityMovementPacket(Vector2 position, Direction direction)
        {
            Position = position;
            Direction = direction;
        }

    }
}

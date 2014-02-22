using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;
using Server.Utils.Math;

namespace Server.Game.Network.Packets.Server
{
    public struct ServerEntityMovementPacket : IPacket
    {
        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_ENTITY_MOVE; }
        }

        public Vector2 Position { get; set; }
        public Direction Direction { get; set; }
        public ulong Id { get; set; }

        public ServerEntityMovementPacket(Vector2 position, Direction direction, ulong id) : this()
        {
            Position = position;
            Direction = direction;
            Id = id;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;
using Server.Utils.Math;

namespace Server.Game.Network.Packets.Server
{
    /// <summary>
    /// When you want to force an update in location and wipe all interpolations, sends this.
    /// </summary>
    public struct ServerEntityTeleportPacket : IPacket
    {
        public ServerEntityTeleportPacket(Vector2 position, ulong id) : this()
        {
            Position = position;
            Id = id;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_ENTITY_TELEPORT; }
        }

        public Vector2 Position { get; set; }

        public ulong Id { get; set; }

    }
}

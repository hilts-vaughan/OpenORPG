using Server.Infrastructure.Network.Packets;
using Server.Infrastructure.World;
using Server.Utils.Math;

namespace Server.Game.Network.Packets.Server
{

    public struct ServerMobMovementPacket : IPacket
    {
        public Vector2 Goal;
        public ulong MobId;

        public ServerMobMovementPacket(ulong mobId, Vector2 goal)
        {
            MobId = mobId;
            Goal = goal;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_MOB_MOVEMENT; }
        }
    }

    /// <summary>
    /// A packet to send to instruct a client that a mob is being created and they should be aware of it's existance.
    /// </summary>
    public struct ServerMobCreatePacket : IPacket
    {
 
        public Entity Mobile { get; set; }

        public ServerMobCreatePacket(Entity mobile) : this()
        {
            Mobile = mobile;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_MOB_CREATE; }
        }
    }
}
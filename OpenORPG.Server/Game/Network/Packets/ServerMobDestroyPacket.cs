using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public struct ServerMobDestroyPacket : IPacket
    {
        public ulong Id;

        public ServerMobDestroyPacket(ulong id)
        {
            Id = id;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_MOB_DESTROY; }
        }
    }
}
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{
    /// <summary>
    /// This packet is sent when an entity is being destroyed and it needs to be replicated across all clients.
    /// This is a notification of intent to destroy to clients. 
    /// </summary>
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
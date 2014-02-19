using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public struct ClientUsePowerPacket : IPacket
    {
        public PowerId PowerId;
        public ulong TargetId;

        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_USE_POWER; }
        }
    }
}
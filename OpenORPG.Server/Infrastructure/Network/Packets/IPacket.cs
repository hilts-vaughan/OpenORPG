using Server.Game.Network.Packets;

namespace Server.Infrastructure.Network.Packets
{
    public interface IPacket
    {
        OpCodes OpCode { get; }
    }
}
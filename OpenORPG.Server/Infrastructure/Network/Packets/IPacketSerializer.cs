namespace Server.Infrastructure.Network.Packets
{
    public interface IPacketSerializer<TSource>
    {
        IPacket Deserialize(TSource data);
        TSource Serialize<TPacket>(TPacket packet) where TPacket : IPacket;
    }
}
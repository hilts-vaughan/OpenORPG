using System;
using Server.Infrastructure.Network.Packets;

namespace Server.Infrastructure.Network.Handlers
{
    public interface IPacketHandler
    {
        void Invoke(GameClient client, IPacket packet);
    }

    public class PacketHandler<T> : IPacketHandler where T : IPacket
    {
        public Action<GameClient, T> Handler;

        public PacketHandler(Action<GameClient, T> handler)
        {
            Handler = handler;
        }

        public void Invoke(GameClient client, IPacket packet)
        {
            Handler(client, (T) packet);
        }
    }
}
using System;
using Fleck;
using Server.Infrastructure.Network.Packets;

namespace Server.Infrastructure.Network
{
    public class NetworkManager
    {
        public static NetworkManager Current;
        private readonly IPacketSerializer<string> _packetSerializer;

        private readonly int _port;
        private WebSocketServer _server;

        private NetworkManager(int port)
        {
            _port = port;
            _packetSerializer = new JsonPacketSerializer();
        }

        public event Action<Connection> Connected;
        public event Action<Connection, IPacket> PacketRecieved;
        public event Action<Connection> Disconnected;


        public static void Create(int port)
        {
            Current = new NetworkManager(port);
        }

        public void Initialize()
        {
            _server = new WebSocketServer(_port, "ws://localhost:" + _port + "/");          
            _server.Start(OnConfig);
        }

        private void OnConfig(IWebSocketConnection socket)
        {
            var connection = new Connection(socket, _packetSerializer);
    
            socket.OnOpen += () => Connected(connection);
            socket.OnClose += () => Disconnected(connection);
            //socket.OnBinary += (data) => DataRecieved(client, data);
            socket.OnMessage += (message) =>
                {
                    IPacket packet = _packetSerializer.Deserialize(message);

                    PacketRecieved(connection, packet);
                };
        }
    }
}
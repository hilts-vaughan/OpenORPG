using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network
{
    /// <summary>
    /// A task that is awaiting to be performed
    /// </summary>
    public struct PacketTask
    {
        public GameClient Client { get; set; }
        public IPacket Packet { get; set; }

        public PacketTask(GameClient client, IPacket packet) : this()
        {
            Client = client;
            Packet = packet;
        }
    }
}

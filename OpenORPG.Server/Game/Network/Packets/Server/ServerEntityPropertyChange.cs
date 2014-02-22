using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{
    public class ServerEntityPropertyChange : IPacket
    {
        /// <summary>
        /// A list of properties to be synced up with the client
        /// </summary>
        public Dictionary<string, dynamic> Properties { get; set; }

        public ulong EntityId { get; set; }

        public ServerEntityPropertyChange(Dictionary<string, dynamic> properties, ulong entityId)
        {
            Properties = properties;
            EntityId = entityId;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_ENTITY_PROPERTY_CHANGE;  }
        }


    }
}

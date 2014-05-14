using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{
    public struct ServerSendQuestOfferPacket : IPacket
    {
        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_SERVER_OFFER_QUEST; }
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool CanGet { get; set; }

        public ServerSendQuestOfferPacket(string name, string description, bool canGet) : this()
        {
            Name = name;
            Description = description;
            CanGet = canGet;
        }
    }
}

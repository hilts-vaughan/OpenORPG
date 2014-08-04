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

        public long QuestId { get; set; }

        public ServerSendQuestOfferPacket(long questId)
            : this()
        {
            QuestId = questId;
        }


    }
}

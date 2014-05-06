using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Client
{

    /// <summary>
    /// A client initiated request to accept a quest
    /// </summary>
    public struct ClientAcceptQuestPacket : IPacket
    {
        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_QUEST_ACCEPT; }
        }

        public long QuestId { get; set; }

        public ClientAcceptQuestPacket(long questId) : this()
        {
            QuestId = questId;
        }
    }
}

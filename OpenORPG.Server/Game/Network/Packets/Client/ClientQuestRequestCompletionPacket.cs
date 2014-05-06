using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;
using Server.Infrastructure.Quests;

namespace Server.Game.Network.Packets.Client
{
    /// <summary>
    /// A client initiated request to complete a <see cref="Quest"/>.
    /// </summary>
    public struct ClientQuestRequestCompletionPacket : IPacket
    {
        public OpCodes OpCode
        {
            get
            {
                return OpCodes.CMSG_QUEST_REQUEST_COMPLETION;
            }
        }

        public long QuestId { get; set; }

        public ClientQuestRequestCompletionPacket(long questId) : this()
        {
            QuestId = questId;
        }
    }
}

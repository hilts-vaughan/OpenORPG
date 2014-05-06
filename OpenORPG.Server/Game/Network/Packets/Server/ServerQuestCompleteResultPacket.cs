using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{
    public enum QuestCompleteResult
    {
        Success,
        CannotGetReward,
        CannotComplete
    }

    public struct ServerQuestCompleteResultPacket : IPacket
    {
        public ServerQuestCompleteResultPacket(QuestCompleteResult result) : this()
        {
            Result = result;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_QUEST_COMPLETE_RESULT; }
        }

        public QuestCompleteResult Result { get; set; }

    }
}

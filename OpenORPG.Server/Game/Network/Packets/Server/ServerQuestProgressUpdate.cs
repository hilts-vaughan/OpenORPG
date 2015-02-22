using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{
    /// <summary>
    /// This packet is sent when an update to the user is required regarding their current progress in a quest.
    /// </summary>
    public struct ServerQuestProgressUpdate : IPacket
    {
        public long QuestId { get; set; }
        public long RequirementIndex { get; set; }
        public long Progress { get; set; }

        public ServerQuestProgressUpdate(long questId, long requirementIndex, long progress) : this()
        {
            QuestId = questId;
            RequirementIndex = requirementIndex;
            Progress = progress;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_QUEST_PROGRESS_UPDATE; }
        }

    }
}

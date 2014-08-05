using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{
    public struct ServerSendQuestListPacket : IPacket
    {
        public List<UserQuestInfo> Quests { get; private set; }

        public ServerSendQuestListPacket(List<UserQuestInfo> quests) : this()
        {
            Quests = quests;
        }


        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_QUEST_SEND_LIST; }
        }
    }
}

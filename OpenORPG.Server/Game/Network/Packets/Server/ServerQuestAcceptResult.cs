using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{
    /// <summary>
    /// A server sent message that indicates to a player the result of their quest acceptance.
    /// Typically, this always returns success except under extreme circumstances. 
    /// </summary>
    public struct ServerQuestAcceptResultPacket : IPacket
    {
        public ServerQuestAcceptResultPacket(bool succeeded) : this()
        {
            Succeeded = succeeded;
        }

        public bool Succeeded { get; set; }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_QUEST_ACCEPT_RESULT; }
        }



    }
}

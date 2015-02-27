using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{
    /// <summary>
    /// A packet message that is sent when the server wishes to notify the player of something.
    /// </summary>
    public struct ServerSendGameMessagePacket : IPacket
    {
        public ServerSendGameMessagePacket(GameMessage messageType, List<string> arguments = null) : this()
        {
            Arguments = arguments;
            MessageType = messageType.ToString();
        }


        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_SEND_GAMEMESSAGE; }        
        }

        /// <summary>
        /// A s list of arguments that are going to be used within the game message to be displayed.
        /// At display time, these are used in place of the {0} tokens inside the game message definitions.
        /// </summary>
        public List<string> Arguments { get; set; }

        /// <summary>
        /// The type of message to be sent
        /// </summary>
        public string MessageType { get; set; }


    }
}

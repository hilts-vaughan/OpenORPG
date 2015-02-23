using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{
    public struct ServerDialogPresentPacket : IPacket
    {
        //TODO: Localize somehow

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_DIALOG_PRESENT;  }
        }

        public string Message { get; private set; }

        public ICollection<String> Links { get; private set; }

        public ServerDialogPresentPacket(string message, ICollection<string> links) : this()
        {
            Message = message;
            Links = links;
        }
    }

}

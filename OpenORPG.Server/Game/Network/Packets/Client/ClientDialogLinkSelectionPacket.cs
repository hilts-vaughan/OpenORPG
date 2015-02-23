using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Client
{
    public struct ClientDialogLinkSelectionPacket : IPacket
    {
        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_DIALOG_LINK_SELECTION; }
        }

        public int LinkId { get; set; }

        public ClientDialogLinkSelectionPacket(int linkId) : this()
        {
            LinkId = linkId;
        }
    }
}

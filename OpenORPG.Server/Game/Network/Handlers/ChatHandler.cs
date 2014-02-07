using System;
using Server.Game.Network.Packets;
using Server.Infrastructure.Network.Handlers;
using Server.Utils;

namespace Server.Game.Network.Handlers
{
    public class ChatHandler
    {
        public const string MessageFormat =
            "<span style='color: gray'>[{0}]&nbsp;<span style='color: cornflowerblue'>{1}</span>:</span>&nbsp;{2}";

        [PacketHandler(OpCodes.CMSG_CHAT_MESSAGE)]
        public static void OnChatMessage(GameClient client, ClientChatMessagePacket packet)
        {
            //TODO: Implement me. :)
        }
    }
}
using System;
using Server.Game.Network.Packets;
using Server.Infrastructure.Network.Handlers;
using Server.Utils;

namespace Server.Game.Network.Handlers
{
    public class ChatHandler
    {
        /// <summary>
        /// Handles incoming chat messages as they come in and processes them accordingly
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(OpCodes.CMSG_CHAT_MESSAGE)]
        public static void OnChatMessage(GameClient client, ClientChatMessagePacket packet)
        {
            var channelId = packet.ChannelId;
            var channel = ChatManager.Current.GetChannel(channelId);
        
            var newPacket = new ServerChatMessagePacket(packet.Message, channelId);
            channel.SendMessage(newPacket);
        }


    }
}
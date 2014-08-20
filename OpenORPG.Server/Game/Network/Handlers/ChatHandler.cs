using System;
using Server.Game.Chat;
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
            var player = client.HeroEntity;
            var zone = player.Zone;

            // Grab the service and fire off to it
            var chatService = zone.GetGameSystem<ChatService>();
            var result = chatService.HandleMessage(player, packet);

            if (!result)
            {
                var channel = ChatManager.Current.GetChannel(player.Zone.ChatChannel.Id);

                packet.Message = packet.Message.Trim();
                if (channel != null && packet.Message != string.Empty)
                {
                    // Strip out any nasty HTML tags that a player might try and inject
                    packet.Message = HtmlTools.StripTagsCharArray(packet.Message);


                    // Send the actual message to the user as requested
                    var newPacket = new ServerChatMessagePacket(player.Name, packet.Message, channel.Id);
                    channel.SendMessage(newPacket);
                }

            }



        }


    }
}
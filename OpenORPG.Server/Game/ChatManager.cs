using System.Collections.Generic;
using System.Linq;
using Server.Game.Network.Packets;

namespace Server.Game
{
    public class ChatChannel
    {
        public List<GameClient> Clients { get; set; }

        public int Id;
        public ChannelType Type;

        public ChatChannel()
        {
            Clients = new List<GameClient>();
        }

        /// <summary>
        /// Broadcasts a message to all users in this chat channel.
        /// </summary>
        /// <param name="messagePacket">The message packet to send to this chat channel</param>
        public void SendMessage(ServerChatMessagePacket messagePacket)
        {
            foreach (var client in Clients)
            {
                client.Send(messagePacket);
            }
        }

        public void Join(GameClient client)
        {            
            client.Send(new ServerJoinChannelPacket(Id, Type, null));
            Clients.Add(client);
        }

        public void Leave(GameClient client)
        {
            //client.Send(new ServerLeaveChatChannelPacket(Id, this.Type, null));
            Clients.Remove(client);
        }
    }

    /// <summary>
    ///     A manager that controls and performs operations related to all <see cref="ChatChannel" />s
    /// </summary>
    public class ChatManager
    {
        public static ChatManager Current;
        private static int _lastId;

        private readonly List<ChatChannel> Channels = new List<ChatChannel>();

        public ChatChannel Global;

        public ChatManager()
        {
            Global = CreateChannel(ChannelType.Global);
        }

        public static void Create()
        {
            Current = new ChatManager();
        }


        public ChatChannel CreateChannel(ChannelType channelType)
        {
            var channel = new ChatChannel();
            channel.Id = _lastId++; 
            channel.Type = channelType;

            Channels.Add(channel);

            return channel;
        }

        public ChatChannel GetChannel(int channelId)
        {
            return Channels.FirstOrDefault(c => c.Id == channelId);
        }
    }
}
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public struct ServerChatMessagePacket : IPacket
    {
        public int ChannelId;
        public string Message;
        public string Sender;

        public ServerChatMessagePacket(string sender, string message, int channelId)
            : this()
        {
            Sender = sender;
            Message = message;
            ChannelId = channelId;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_CHAT_MESSAGE; }
        }
    }
}
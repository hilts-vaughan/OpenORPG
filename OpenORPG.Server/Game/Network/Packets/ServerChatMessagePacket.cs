using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public struct ServerChatMessagePacket : IPacket
    {
        public int ChannelId;
        public string Message;


        public ServerChatMessagePacket(string message, int channelId)
            : this()
        {
            Message = message;
            ChannelId = channelId;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_CHAT_MESSAGE; }
        }
    }
}
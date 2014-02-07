using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public struct ClientChatMessagePacket : IPacket
    {
        public int ChannelId;
        public string Message;

        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_CHAT_MESSAGE; }
        }
    }
}
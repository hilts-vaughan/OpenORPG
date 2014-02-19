using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public struct ServerJoinChannelPacket : IPacket
    {
        public int ChannelId;

        public string ChannelName;
        public ChannelType ChannelType;

        public ServerJoinChannelPacket(int channelId, ChannelType channelType, string channelName)
        {
            ChannelId = channelId;
            ChannelType = channelType;
            ChannelName = channelName;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_JOIN_CHANNEL; }
        }
    }
}
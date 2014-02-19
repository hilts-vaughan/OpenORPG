using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public struct ServerHeroCreateResponsePacket : IPacket
    {
        public HeroStatus Status;

        public ServerHeroCreateResponsePacket(HeroStatus heroStatus)
        {
            Status = heroStatus;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_HERO_CREATE_RESPONSE; }
        }
    }
}
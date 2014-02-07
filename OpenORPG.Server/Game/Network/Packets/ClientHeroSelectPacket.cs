using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public struct ClientHeroSelectPacket : IPacket
    {
        public int HeroId { get; set; }


        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_HERO_SELECT; }
        }
    }
}
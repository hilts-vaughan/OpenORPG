using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public struct ClientHeroCreatePacket : IPacket
    {
        public string Name;


        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_HERO_CREATE; }
        }
    }
}
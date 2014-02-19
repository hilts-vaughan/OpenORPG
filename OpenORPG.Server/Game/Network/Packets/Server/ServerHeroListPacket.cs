using System.Collections.Generic;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    /// <summary>
    ///     Contains information about a particular hero
    /// </summary>
    public struct HeroInfo
    {
        public int HeroId;
        public string Name;
    }

    public struct ServerHeroListPacket : IPacket
    {
        /// <summary>
        ///     A list of heroes associated with this list
        /// </summary>
        public List<HeroInfo> Heroes;

        public ServerHeroListPacket(List<HeroInfo> heroes)
        {
            Heroes = heroes;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_HERO_LIST; }
        }
    }
}
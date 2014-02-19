using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public enum HeroStatus
    {
        OK = 1,

        /// <summary>
        /// This indicates that a character is invalid. This is ususally sent if someone sends
        /// a hero ID that does not exist on the server.
        /// </summary>
        Invalid = 2,

        /// <summary>
        /// This is used if the character cannot be retrieved. This is usually due to a piece of
        /// data about them going missing or something similar. 
        /// If something fails during the loading process, send this.
        /// </summary>
        Unavailable = 3
    }

    public struct ServerHeroSelectResponsePacket : IPacket
    {
        public HeroStatus Status;

        public ServerHeroSelectResponsePacket(HeroStatus heroStatus)
        {
            Status = heroStatus;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_HERO_SELECT_RESPONSE; }
        }
    }
}
using System.Collections.Generic;
using Server.Infrastructure.Network.Packets;
using Server.Infrastructure.World;

namespace Server.Game.Network.Packets
{
    public struct ServerZoneChangedPacket : IPacket
    {
        /// <summary>
        /// This is your unique ID on this map, you can use this to locate the connecting
        /// players entity on the client for this zone.
        /// </summary>
        public ulong HeroId { get; private set; }

        /// <summary>
        /// The ZoneId to send down to the player
        /// </summary>
        public long ZoneId { get; private set; }

        public List<Entity> Entities { get; private set; } 

        public ServerZoneChangedPacket(long zoneId, ulong heroId, List<Entity> entities )
            : this()
        {
            HeroId = heroId;
            ZoneId = zoneId;
            Entities = entities;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_ZONE_CHANGED; }
        }
    }
}
using Server.Game.Entities;
using Server.Game.Zones;

namespace Server.Game.Utility
{
    public class UserUtility
    {

        /// <summary>
        /// Returns the zone object that a player belongs to at the given moment.
        /// </summary>
        /// <param name="player">The player we wish to lookup</param>
        /// <returns></returns>
        public static Zone GetZoneForPlayer(Player player)
        {
            return ZoneManager.Instance.FindZone(player.ZoneId);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using Server.Game.Entities;
using Server.Game.Network.Packets.Server;
using Server.Game.Zones;
using Server.Utils.Math;

namespace Server.Game.Chat.Handlers
{
    /// <summary>
    /// Parses some commands and then allows the <see cref="ChatService"/>
    /// </summary>
    public static class ChatCommandHandler
    {

        [ChatHandler("Logout")]
        public static void LogoutCommandHandler(Player player, List<string> arguments)
        {
                player.Client.Connection.Disconnect("The player logged out forcefully.");            
        }

        [ChatHandler("Time")]
        public static void ServerTimeHandler(Player player, List<string> arguments)
        {
            // Replies with the current server time
            var time = DateTime.Now;
            player.Client.Send(new ServerSendGameMessagePacket(GameMessage.CurrentTime, new List<string>() { time.ToShortDateString()}));
        }


        // The below are admin commands, they should only be used by those with the permissions required to do so
        [ChatHandler("Warpto")]
        public static void WarpToHandler(Player player, List<string> arguments)
        {
            // Get the map ID an argument, the other parameters are optional                
            var zoneId = int.Parse(arguments[0]);
            
            int x = player.X;
            int y = player.Y;

            // If we had exactly 3 parameters (must have had a X, Y as well)
            if (arguments.Count == 3)
            {
                x = int.Parse(arguments[1]);
                y = int.Parse(arguments[2]);
            }

            // Move the player to the zone they have requested
            ZoneManager.Instance.SwitchToZoneAndPosition(player, zoneId, new Vector2(x, y));

        }

        [ChatHandler("Teleport")]
        public static void MoveToHandler(Player player, List<string> arguments)
        {
            var x = int.Parse(arguments[0]);
            var y = int.Parse(arguments[1]);

            player.Teleport(new Vector2(x, y));        
        }

        [ChatHandler("Heal")]
        public static void FullHeal(Player player, List<string> arguments)
        {
            player.CharacterStats[StatTypes.Hitpoints].CurrentValue =
                player.CharacterStats[StatTypes.Hitpoints].MaximumValue;
        }



    }
}

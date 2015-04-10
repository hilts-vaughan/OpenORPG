using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Game.Movement;
using Server.Game.Network.Packets;
using Server.Game.Network.Packets.Client;
using Server.Game.Network.Packets.Server;
using Server.Game.Utility;
using Server.Game.Zones;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Network.Handlers;
using Server.Utils.Math;

namespace Server.Game.Network.Handlers
{
    /// <summary>
    /// All packets related to movement data can be found here
    /// </summary>
    public class MovementHandler
    {
        private const int LenianceFactor = 150;

        [PacketHandler(OpCodes.CMSG_MOVEMENT_REQUEST)]
        public static void OnChatMessage(GameClient client, ClientMovementRequestPacket packet)
        {
            var player = client.HeroEntity;
            var zone = client.HeroEntity.Zone;

            if (zone == null)
                return;

            var requestedPosition = packet.CurrentPosition;
            var direction = packet.Direction;

            var distance = Vector2.Distance(requestedPosition, player.Position);
            // Ignore packets claiming to make large leaps and bounds
            if (distance > LenianceFactor)
            {
                
                Logger.Instance.Warn("The player moved far too much in one frame. Could be lag or a sign of hacking. Attempting to move back");
                
                // Send the player back to their old position, correcting them
                player.Teleport(player.Position);
                return;
                //client.Connection.Disconnect("Hacking Attempt: Movement pulse exceeded");
            }

            // Set the state
            if (packet.Terminates)
                player.CharacterState = CharacterState.Idle;
            else
                player.CharacterState = CharacterState.Moving;

            // Move the player and set direction
            player.Direction = packet.Direction;
            player.Position = requestedPosition;

        }

        [PacketHandler(OpCodes.CMSG_CLICK_WARP_REQUEST)]
        public static void OnClickWarpRequest(GameClient client, ClientClickWarpRequest packet)
        {
            var player = client.HeroEntity;
            var zone = client.HeroEntity.Zone;

            player.Teleport(new Vector2(packet.X, packet.Y));

        }

        [PacketHandler(OpCodes.CMMSG_ZONE_CHANGE)]
        public static void OnZoneChangeRequest(GameClient client, ClientZoneChangeRequestPacket packet)
        {
            // Check if leaving is legal
            var canLeave = client.HeroEntity.Zone.CanLeave(packet.Direction, client.HeroEntity);

            if (canLeave)
            {
                // Alert the console
                Logger.Instance.Info("{0} is changing zones...", client.HeroEntity.ToString());

                // Fetch the zone we'll be transferring to
                var zoneId = client.HeroEntity.Zone.ZoneExitPoints[(int)packet.Direction];
                var zone = ZoneManager.Instance.FindZone(zoneId);

                // Check for existence first before trying to transfer 
                if (zone == null)
                {
                    Logger.Instance.Error("{0} tried to transfer to {1} from {2} but the zone could not be found.", client.HeroEntity.Name, client.HeroEntity.Zone.Id, zoneId);
                    return;
                }
                var newPos = GetFreePositionInZoneFromDirection(packet.Direction, zone, client.HeroEntity.Position);

                ZoneManager.Instance.SwitchToZoneAndPosition(client.HeroEntity, zone, newPos);
                client.HeroEntity.Direction = packet.Direction;
            }
        }

        /// <summary>
        /// Gets the next free position available in the map given a direction entering from.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="zone"></param>
        /// <returns></returns>
        private static Vector2 GetFreePositionInZoneFromDirection(Direction direction, Zone zone, Vector2 oldPosition)
        {
            float x, y;

            switch (direction)
            {
                case Direction.North:
                    x = oldPosition.X;
                    y = ((zone.TileMap.Height - 4) * zone.TileMap.TileHeight);
                    return new Vector2(x, y);

                case Direction.East:
                    x = zone.TileMap.TileWidth * 4;
                    y = oldPosition.Y;
                    return new Vector2(x, y);

                case Direction.South:
                    x = oldPosition.X;
                    y = 0 + zone.TileMap.TileHeight * 4;
                    return new Vector2(x, y);

                case Direction.West:
                    x = ((zone.TileMap.Width - 4) * zone.TileMap.TileWidth);
                    y = oldPosition.Y;
                    return new Vector2(x, y);
            }

            Logger.Instance.Error("Took an illegal branch, check direction switch.");
            return new Vector2(0, 0);
        }


    }
}

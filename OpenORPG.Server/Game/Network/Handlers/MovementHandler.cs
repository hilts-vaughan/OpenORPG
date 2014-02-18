﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Movement;
using Server.Game.Network.Packets;
using Server.Game.Network.Packets.Server;
using Server.Game.Utility;
using Server.Infrastructure.Network.Handlers;

namespace Server.Game.Network.Handlers
{
    /// <summary>
    /// All packets related to movement data can be found here
    /// </summary>
   public class MovementHandler
    {


       [PacketHandler(OpCodes.CMSG_MOVEMENT_REQUEST)]
       public static void OnChatMessage(GameClient client, ClientMovementRequestPacket  packet)
       {
           var player = client.HeroEntity;
           var zone = client.HeroEntity.Zone;

           var requestedPosition = packet.CurrentPosition;
           var direction = packet.Direction;

           //TODO: We should do some sanity checking on this position, check if it's legal
           // Should also probably compare to the old position and make sure it seems reasonable
           player.Position = requestedPosition;

           var newPacket = new ServerEntityMovementPacket(requestedPosition, direction);
           zone.SendToEntitiesInRange(newPacket, player);
       }
        
    }
}
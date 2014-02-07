using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Movement;
using Server.Game.Network.Packets;
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
           client.HeroEntity.Position = packet.CurrentPosition;

           var zone = UserUtility.GetZoneForPlayer(client.HeroEntity);

       }
        
    }
}

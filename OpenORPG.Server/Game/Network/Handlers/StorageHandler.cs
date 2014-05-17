using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Network.Packets;
using Server.Game.Network.Packets.Client;
using Server.Game.Network.Packets.Server;
using Server.Infrastructure.Network.Handlers;

namespace Server.Game.Network.Handlers
{
    /// <summary>
    /// A simple static handler that can be used for handling storage related techniques. 
    /// 
    /// Requests for moving and interacting with storage will be here.
    /// </summary>
    public static class StorageHandler
    {

        [PacketHandler(OpCodes.CMSG_STORAGE_MOVE_SLOT)]
        public static void OnStorageSlotMoveRequest(GameClient client, ClientStorageMoveSlotPacket packet)
        {
            var hero = client.HeroEntity;

            bool moveSuccessful = false;

            switch (packet.StorageType)
            {
                case StorageType.Inventory:
                    moveSuccessful = hero.Backpack.MoveTo(packet.SourceSlot, packet.DestSlot);
                    break;
            }

            // Send a full inventory update to the client
            var outboundPacket = new ServerSendHeroStoragePacket(hero.Backpack, packet.StorageType);
            client.Send(outboundPacket);

        }

        [PacketHandler(OpCodes.CMSG_STORAGE_DROP)]
        public static void OnStorageDropRequest(GameClient client, ClientDropStorageRequestPacket packet)
        {
            var hero = client.HeroEntity;

            for (int i = 0; i < packet.Amount; i++)
                hero.Backpack.RemoveSingleAt(packet.SlotId);

            var outboundPacket = new ServerSendHeroStoragePacket(hero.Backpack, StorageType.Inventory);
            client.Send(outboundPacket);

        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Game.Items;
using Server.Game.Items.Equipment;
using Server.Game.Network.Packets;
using Server.Game.Network.Packets.Client;
using Server.Game.Network.Packets.Server;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Network.Handlers;

namespace Server.Game.Network.Handlers
{
    /// <summary>
    /// Handles requests regarding items
    /// </summary>
    public static class ItemHandler
    {

        /// <summary>
        /// Handles an equipment set request by a player.
        /// Performs checks that ensures the integriy of the request and ensures everything is valid.
        /// 
        /// Removes item from user inventory as well.
        /// 
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(OpCodes.CMSG_ITEM_USE)]
        public static void OnEquipRequest(GameClient client, ClientHeroUseItemPacket packet)
        {
            var hero = client.HeroEntity;
            var itemFromSlot = hero.Backpack.GetItemInfoAt(packet.SlotId).Item;

            if (itemFromSlot != null)
            {

                //todo: remove this special case, make it part of the 'use' section
                if (itemFromSlot is Equipment)
                {
                    EquipFromSlotIdInInventory(packet, hero);      
                }
  
                // Attempt to perform the use of the item on self
                itemFromSlot.UseItemOn(hero, hero);

                if(itemFromSlot.Consumable)
                    hero.RemoveFromBackpack((int) packet.SlotId, 1);

            }
        }

        [PacketHandler(OpCodes.CMSG_UNEQUIP_ITEM)]
        public static void OnUnequipRequest(GameClient client, ServerUnequipItemPacket packet)
        {
            var hero = client.HeroEntity;
            hero.RemoveEquipment(packet.Slot);
        }


        private static void EquipFromSlotIdInInventory(ClientHeroUseItemPacket packet, Player hero)
        {
            // Attempt to equip the item
            var success = hero.TryEquipItem(packet.SlotId);
        }




    }
}

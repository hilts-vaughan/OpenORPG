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

                if (itemFromSlot is Equipment)
                {
                    EquipFromSlotIdInInventory(packet, hero);
                }

                else if (itemFromSlot is FieldItem)
                {
                    // Item is probably a field item, use it like that
                }

                else
                {
                    // Do nothing and let the user know they can't do that right now
                    var message = new ServerSendGameMessagePacket(GameMessage.ItemCannotUse);
                    client.Send(message);

                }


            }
        }

        private static void DeequipFromEquipmentSlot(ClientHeroUseItemPacket packet, Player hero)
        {
            //var equipmentIndexer = (int)packet.EquipmentSlot;

            //if (hero.Equipment[equipmentIndexer] != null && !hero.Backpack.IsFull)
            //{
            //    // Remove equipment
            //    var equipment = hero.Equipment[equipmentIndexer];
            //    hero.Equipment[equipmentIndexer] = null;

            //    // Add back into inventory
            //    var success = hero.Backpack.TryAddItem(equipment);
            //}


        }
        private static void EquipFromSlotIdInInventory(ClientHeroUseItemPacket packet, Player hero)
        {
            var itemInInventory = hero.Backpack.GetItemInfoAt(packet.SlotId).Item as Equipment;


            if (itemInInventory != null)
            {
                // Remove the item from the backpack
                hero.Backpack.RemoveItemAt(packet.SlotId);

                // Assign it onto the hero
                hero.Equipment[(int)itemInInventory.Slot] = itemInInventory;

                Logger.Instance.Info("{0} has equipped a {1}.", hero.Name, itemInInventory.Name);
            }
            else
                Logger.Instance.Warn("{0} tried to equip an item that they did not have.", hero.Name);
        }




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Game.Items.Equipment;
using Server.Game.Network.Packets;
using Server.Game.Network.Packets.Client;
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
        /// If a slot number of -1 is provided, the item will be removed from the specified slot.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(OpCodes.CMSG_HERO_EQUIP)]
        public static void OnEquipRequest(GameClient client, ClientHeroEquipItemPacket packet)
        {
            var hero = client.HeroEntity;

            if (packet.SlotId > -1)
                EquipFromSlotIdInInventory(packet, hero);
            else
                DeequipFromEquipmentSlot(packet, hero);
        }

        private static void DeequipFromEquipmentSlot(ClientHeroEquipItemPacket packet, Player hero)
        {
            var equipmentIndexer = (int)packet.EquipmentSlot;

            if (hero.Equipment[equipmentIndexer] != null && !hero.Backpack.IsFull)
            {
                // Remove equipment
                var equipment = hero.Equipment[equipmentIndexer];
                hero.Equipment[equipmentIndexer] = null;

                // Add back into inventory
                hero.Backpack.AddItem(equipment);
            }


        }
        private static void EquipFromSlotIdInInventory(ClientHeroEquipItemPacket packet, Player hero)
        {
            var itemInInventory = hero.Backpack.GetItemAt(packet.SlotId) as Equipment;


            if (itemInInventory != null)
            {
                // Ensure that this can be equipped here
                if (itemInInventory.Slot.HasFlag(packet.EquipmentSlot))
                {
                    // Remove the item from the backpack
                    hero.Backpack.RemoveItemAt(packet.SlotId);

                    // Assign it onto the hero
                    hero.Equipment[(int)packet.EquipmentSlot] = itemInInventory;

                    Logger.Instance.Info("{0} has equipped a {1}.", hero.Name, itemInInventory.Name);

                }
                else
                    Logger.Instance.Warn("{0} tried to equip an item in the wrong slot. Denied", hero.Name);
            }
            else
                Logger.Instance.Warn("{0} tried to equip an item that they did not have.", hero.Name);
        }




    }
}

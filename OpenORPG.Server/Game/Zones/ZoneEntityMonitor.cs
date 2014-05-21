using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Game.Items.Equipment;
using Server.Game.Network.Packets.Client;
using Server.Game.Network.Packets.Server;
using Server.Infrastructure.Logging;
using Server.Infrastructure.World;
using Server.Infrastructure.World.Systems;

namespace Server.Game.Zones
{
    /// <summary>
    ///  An object with the sole responsibility of delegating game synchronization between clients. 
    ///  This class is responsible for broadcasting the state changes and events that occur in the game
    ///  state to the clients that care about these changes. 
    /// 
    ///  This includes things equipment notifications, state changes and the like.
    /// </summary>
    public class ZoneEntityMonitor : GameSystem
    {
        public ZoneEntityMonitor(Zone world)
            : base(world)
        {

        }

        public override void Update(float frameTime)
        {
            // Do nothing on purpose, this system has no business doing anything but firing off dumb updates
        }

        public override void OnEntityAdded(Entity entity)
        {
            if (entity is Player)
                MonitorPlayer(entity as Player);
        }

        public override void OnEntityRemoved(Entity entity)
        {
            if (entity is Player)
                NeglectPlayer(entity as Player);
        }

        //NOTICE: PLEASE CLEANUP ANYTHING YOU REGISTER HERE, OTHERWISE THINGS WILL GET CHAOTIC

        private void MonitorPlayer(Player player)
        {
            player.EquipmentChanged += PlayerOnEquipmentChanged;
            player.CharacterStats.CurrentValueChanged += CharacterStatsOnCurrentValueChanged;
        }

        private void CharacterStatsOnCurrentValueChanged(long oldValue, long newValue, StatTypes statType, Character tracking)
        {
            var player = tracking as Player;
            Logger.Instance.Debug("{0} has {1} changed to {2}", player.Name, statType, newValue);

            //TODO: Revaluate this, right now we only broadcast hitpoint changes
            if (statType == StatTypes.Hitpoints)
            {
                var zone = player.Zone;
                var updatePacket = new ServerCharacterStatChange(statType, newValue,
                    player.CharacterStats[statType].MaximumValue,
                    (long)player.Id);

                // Broadcast to interested parties
                zone.SendToEveryone(updatePacket);
            }

        }



        private void NeglectPlayer(Player player)
        {
            player.EquipmentChanged -= PlayerOnEquipmentChanged;
            player.CharacterStats.CurrentValueChanged -= CharacterStatsOnCurrentValueChanged;
        }

        private void PlayerOnEquipmentChanged(Equipment equipment, Player player, EquipmentSlot slot)
        {
            var request = new ServerEquipmentUpdatePacket(equipment, slot);
            player.Client.Send(request);

            var inventoryUpdate = new ServerSendHeroStoragePacket(player.Backpack, StorageType.Inventory);
            player.Client.Send(inventoryUpdate);
        }






    }
}

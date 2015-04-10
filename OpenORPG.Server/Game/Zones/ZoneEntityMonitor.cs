using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using Server.Game.Combat;
using Server.Game.Combat.Effects;
using Server.Game.Database.Models;
using Server.Game.Entities;
using Server.Game.Items;
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
    public class ZoneEntityMonitorSystem : GameSystem
    {
        public ZoneEntityMonitorSystem(Zone world)
            : base(world)
        {

        }

        public override void Update(double frameTime)
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
            player.LearnedSkill += PlayerOnLearnedSkill;
            player.BackpackChanged += PlayerOnBackpackChanged;
            
            
            player.AcceptedQuest += PlayerOnAcceptedQuest;          

            player.ExperienceChanged += PlayerOnExperienceChanged;
            player.LevelChanged += PlayerOnLevelChanged;            
        }

        private void PlayerOnAcceptedQuest(Player player)
        {
            var questUpdate = new ServerSendQuestListPacket(player.QuestLog);
            player.Client.Send(questUpdate);
        }

        private void PlayerOnBackpackChanged(Player player)
        {
            var inventoryUpdate = new ServerSendHeroStoragePacket(player.Backpack, StorageType.Inventory);
            player.Client.Send(inventoryUpdate);
        }

        private void ActiveStatusEffectsOnEffectAdded(Player player, StatusEffect effect)
        {
            throw new NotImplementedException();
        }

        private void PlayerOnLevelChanged(int newValue, int oldValue, Player player)
        {
            // Send a notification to everyone in the zone
            player.Zone.ChatChannel.SendMessage(GameMessage.LevelUp, new List<string>()
            {
                (player.Name).ToString(),
                player.Level.ToString()
            });
        }

        private void PlayerOnExperienceChanged(int newValue, int oldValue, Player player)
        {
            if (newValue > oldValue)
            {

                var packet = new ServerSendGameMessagePacket(GameMessage.GainExperience, new List<string>()
                {
                    (newValue - oldValue).ToString()
                });

                player.Client.Send(packet);
            }
        }

     
    
        /// <summary>
        /// This is called to synchronize this new fact to the player
        /// </summary>
        /// <param name="skill">The skill that was learned and to be synced</param>
        /// <param name="player">The player that has learned this skill</param>
        private void PlayerOnLearnedSkill(Skill skill, Player player)
        {
            // Notify the player that they learned this skill, send it over
            var packet = new ServerSkillChangePacket(player.Skills);
            player.Client.Send(packet);

            Logger.Instance.Info("{0} has learned the skill {1}.", player.Name, skill.SkillTemplate.Name);
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
            player.LearnedSkill -= PlayerOnLearnedSkill;
            player.BackpackChanged -= PlayerOnBackpackChanged;

            
            //TODO: Re-enable me
            player.AcceptedQuest -= PlayerOnAcceptedQuest;
            
            player.ExperienceChanged -= PlayerOnExperienceChanged;
            player.LevelChanged -= PlayerOnLevelChanged;
        }

        private void PlayerOnEquipmentChanged(Equipment equipment, Player player, EquipmentSlot slot)
        {
            var request = new ServerEquipmentUpdatePacket(equipment, slot);
            player.Client.Send(request);


            // Send notification to the client
            var request2 = new ServerSendGameMessagePacket(GameMessage.EquipmentChanged);
            player.Client.Send(request2);
        }






    }
}

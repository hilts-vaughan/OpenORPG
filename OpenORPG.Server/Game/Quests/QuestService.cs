using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models;
using Server.Game.Entities;
using Server.Game.Items;
using Server.Game.Storage;
using Server.Game.Zones;
using Server.Infrastructure.World;
using Server.Infrastructure.World.Systems;

namespace Server.Game.Quests
{

    /// <summary>
    /// A service responsible for tracking quest related criteria. 
    /// </summary>
    public class QuestService : GameSystem
    {
        // A simple tracker for tracking player quests
        private QuestRequirementTracker _questRequirementTracker = new QuestRequirementTracker();

        // These are lookup tables for event handlers; do not modify
        private Dictionary<Player, ItemStorage.ItemEvent> _backpackActions = new Dictionary<Player, ItemStorage.ItemEvent>();

        public QuestService(Zone world)
            : base(world)
        {
        }


        public override void Update(float frameTime)
        {


        }

        public override void OnEntityAdded(Entity entity)
        {

            if (entity is Monster)
                _questRequirementTracker.OnMonsterAdded(entity as Monster);

            if (entity is Player)
                OnPlayerAdded(entity as Player);
        }

        public override void OnEntityRemoved(Entity entity)
        {
            if (entity is Monster)
                _questRequirementTracker.OnMonsterRemoved(entity as Monster);

            if (entity is Player)
                OnPlayerRemoved(entity as Player);

        }

        private void OnPlayerAdded(Player player)
        {
            // Track stuff that quest requirements might need updating on
            player.AcceptedQuest += PlayerOnAcceptedQuest;

            // Add a player backpack event
            ItemStorage.ItemEvent action = (item, slotId, amount) => BackpackOnItemAdded(player, item, slotId, amount);
            player.Backpack.ItemAdded += action;
            _backpackActions.Add(player, action);


            _questRequirementTracker.LoadPlayer(player);
        }

        private void BackpackOnItemAdded(Player player, Item item, long slotId, long amount)
        {
            // Send an update if it's required
        }

        private void OnPlayerRemoved(Player player)
        {
            player.AcceptedQuest -= PlayerOnAcceptedQuest;
            
            // Remove items from handlers where required
            player.Backpack.ItemAdded -= _backpackActions[player];


            _questRequirementTracker.UnloadPlayer(player);
        }

        private void PlayerOnAcceptedQuest(UserQuestInfo userQuestInfo, Player player)
        {
            _questRequirementTracker.NotifyBeginTracking(userQuestInfo, player);
        }




    }
}

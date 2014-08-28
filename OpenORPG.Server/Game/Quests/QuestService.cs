using System;
using System.Collections.Generic;
using Server.Game.Database.Models;
using Server.Game.Entities;
using Server.Game.Items;
using Server.Game.Network.Packets.Server;
using Server.Game.Storage;
using Server.Game.Zones;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Quests.Trackers;
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
        private QuestRequirementTrackerObsolete _questRequirementTrackerObsolete = new QuestRequirementTrackerObsolete();

        // These are lookup tables for event handlers; do not modify
        private Dictionary<Player, ItemStorage.ItemEvent> _backpackActions = new Dictionary<Player, ItemStorage.ItemEvent>();
        private Dictionary<Player, ItemStorage.ItemEvent> questActions = new Dictionary<Player, ItemStorage.ItemEvent>();

        private List<IQuestRequirementTracker> _questRequirementTrackers = new List<IQuestRequirementTracker>();

        public QuestService(Zone world)
            : base(world)
        {

            // Create some trackers here as required
            _questRequirementTrackers.Add(new MonsterKillCountQuestRequirementTracker(world));

            // Hook up event handlers
            _questRequirementTrackers.ForEach(x => x.ProgressChanged += OnProgressChanged);

        }

        private void OnProgressChanged(Player player, QuestLogEntry entry, int index, int progress)
        {
            // We can choose to give rewards here when all requirements have suddenly been met
            var step = entry.CurrentStep;

            if (step.IsRequirementsMet(player, entry.GetProgress()))
            {
                if (!entry.IsLastStep)
                {
                    // Advance the step and take any requirements
                    step.TakeRequirements(player);
                    entry.AdvanceStep();
                }

                    // If is last step, only advance if we can give reward
                else
                {
                    var canGiveRewards = entry.Quest.CanGiveReward(player);
                    
                    if (canGiveRewards)
                    {
                        step.TakeRequirements(player);
                        entry.AdvanceStep();
                    }

                    else
                    {
                        // Let them know for some reason the rewards were not met
                        var message = new ServerSendGameMessagePacket(GameMessage.QuestCannotGiveReward);
                        player.Client.Send(message);
                    }

                }

            }

            // If there's no more steps possible to get through, give the reward
            if (entry.CurrentStep == null)
            {
                // Attempt to complete the quest
                var success = entry.Quest.TryCompleteQuest(player);

                if (success)
                {
                    var message = new ServerSendGameMessagePacket(GameMessage.QuestCompleted, new List<string>() { entry.Quest.Name});
                    player.Client.Send(message);
                }
                
                else
                {
                    var message = new ServerSendGameMessagePacket(GameMessage.QuestCannotGiveReward);
                    player.Client.Send(message);
                }


            }

        }


        public override void Update(float frameTime)
        {


        }

        public override void OnEntityAdded(Entity entity)
        {

            _questRequirementTrackers.ForEach(x => x.OnEntityAdded(entity));

            if (entity is Player)
                OnPlayerAdded(entity as Player);
        }

        public override void OnEntityRemoved(Entity entity)
        {
            _questRequirementTrackers.ForEach(x => x.OnEntityRemoved(entity));

            if (entity is Player)
                OnPlayerRemoved(entity as Player);

        }

        private void OnPlayerAdded(Player player)
        {
            // Track stuff that quest requirements might need updating on
            QuestLog.QuestLogEvent questAction = (entry) => QuestInfoOnQuestAccepted(entry, player);
            player.QuestLog.QuestAccepted += questAction;

            // Add a player backpack event
            ItemStorage.ItemEvent action = (item, slotId, amount) => BackpackOnItemAdded(player, item, slotId, amount);
            player.Backpack.ItemAdded += action;
            _backpackActions.Add(player, action);


            _questRequirementTrackerObsolete.LoadPlayer(player);
        }

        private void BackpackOnItemAdded(Player player, Item item, long slotId, long amount)
        {

        }

        private void QuestInfoOnQuestAccepted(QuestLogEntry entry, Player player)
        {
            //_questRequirementTracker.NotifyBeginTracking(entry., player);
        }



        private void OnPlayerRemoved(Player player)
        {
            //TODO: Players will need to be registered here for certain stuff
            //player.AcceptedQuest -= PlayerOnAcceptedQuest;

            // Remove items from handlers where required
            player.Backpack.ItemAdded -= _backpackActions[player];
            _backpackActions.Remove(player);

            _questRequirementTrackerObsolete.UnloadPlayer(player);
        }



        /// <summary>
        /// Checks a specified log entry (typically after performing an action that might be reflect progress changes)
        /// and affects the state accordingly.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="entry"></param>
        private void CheckQuestEntryProgress(Player player, QuestLogEntry entry)
        {
            if (entry.CurrentStep != null)
            {
                var requirementsMet = entry.CurrentStep.IsRequirementsMet(player, entry.GetProgress());

                if (requirementsMet)
                {
                    entry.AdvanceStep();
                    Logger.Instance.Info("Advancing step in quest #{0}; player met requirements", player, entry.Quest.QuestId);
                }

            }

        }



    }
}

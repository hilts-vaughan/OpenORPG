using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Database.Models.Quests;
using Server.Game.Entities;
using Server.Game.Network.Packets.Server;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Quests;

namespace Server.Game.Quests
{
    /// <summary>
    /// A singelton quest manager
    /// </summary>
    public class QuestManager
    {
        private static QuestManager _instance;

        protected QuestManager()
        {

        }

        public static QuestManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new QuestManager();
                return _instance;
            }
        }


        public bool CanPlayerGetQuest(Quest quest, Player player)
        {
            // If the player has the quest completed already, decline
            foreach (var entry in player.QuestLog)
            {
                if (entry.Quest.QuestId == quest.QuestId && entry.State != QuestState.Available)
                    return false;
            }

            // Check if the player
            foreach (var requirement in quest.StartRequirements)
            {
                if (!requirement.HasRequirements(player))
                    return false;
            }


            // The player can flag this quest up
            return true;
        }

        /// <summary>
        /// Gives a player a quest. If the quest was already completed, it will be marked as in progress once again.
        /// If the quest has not been seen before, it will be marked as such.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="player"></param>
        public void GivePlayerQuest(Quest quest, Player player)
        {

            // Attempt to give a quest to the player
            player.QuestLog.TryAddQuest(quest);

            // Send a notification to let them know
            var message = new ServerSendGameMessagePacket(GameMessage.NewQuest);
            player.Client.Send(message);

            Logger.Instance.Info("{0} has been given the quest {1} [#{2}]", player.Name, quest.Name, quest.QuestId);
        }

        /// <summary>
        /// Attempts to finish a quest that a player is attempting to turn in.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool TryFinishQuest(Quest quest, Player player)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a quest that has been created
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public QuestTemplate GetQuest(long id)
        {
            using (var context = new GameDatabaseContext())
            {

                var quest = context.Quests.First(x => x.QuestTemplateId == id);

                //context.Entry(quest).Reference(x => x.EndMonsterRequirements).Load();
                //context.Entry(quest).Reference(x => x.EndItemRequirements).Load();
                context.Entry(quest).Collection(x => x.RewardItems).Load();


                return quest;
            }

        }
    }
}

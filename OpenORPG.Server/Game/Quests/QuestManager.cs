using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models;
using Server.Game.Entities;
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
            foreach (var questInfo in player.QuestInfo)
            {
                if (questInfo.QuestId == quest.QuestId && questInfo.State != QuestState.Available)
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
            var questInfo = player.QuestInfo.FirstOrDefault(x => x.QuestId == quest.QuestId);

            // If the user already has this quest, reset the flag... otherwise add it
            if (questInfo == null)
            {
                var newEntry = new UserQuestInfo() { QuestId = quest.QuestId, State = QuestState.InProgress };
                player.QuestInfo.Add(newEntry);
            }
            else
            {
                questInfo.State = QuestState.InProgress;
            }

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


    }
}

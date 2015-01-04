using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.DAL;
using OpenORPG.Database.Enums;
using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Entities;
using Server.Infrastructure.Quests;

namespace Server.Game.Quests
{
    /// <summary>
    /// A quest list contains a listing of quest information for a particular user, <see cref="Player"/>.
    /// </summary>
    public class QuestLog : IEnumerable<QuestLogEntry>
    {
        private List<QuestLogEntry> _quests = new List<QuestLogEntry>();


        public delegate void QuestLogEvent(QuestLogEntry entry);

        public event QuestLogEvent QuestAccepted;

        protected virtual void OnQuestAccepted(QuestLogEntry entry)
        {
            QuestLogEvent handler = QuestAccepted;
            if (handler != null) handler(entry);
        }

        public QuestLog(IEnumerable<UserQuestInfo> questInfoesInfos)
        {

            foreach (var questInfo in questInfoesInfos)
            {
                var entry = new QuestLogEntry(new Quest(QuestManager.Instance.GetQuest(questInfo.QuestId)), questInfo);
                AddEntry(entry);
            }

        }


        /// <summary>
        /// Tries to add a quest to the current log. If the quest already exists or can't be triggered, false will be returned.
        /// </summary>
        /// <param name="quest">The quest to attempt to add to the log</param>
        /// <returns></returns>
        public bool TryAddQuest(Quest quest)
        {
            var questEntry = GetQuestLogEntry(quest.QuestId);
            if (questEntry == null)
            {
                var info = new UserQuestInfo();
                info.QuestId = quest.QuestId;
                var entry = new QuestLogEntry(quest, info);
                AddEntry(entry);
                OnQuestAccepted(entry);
                return true;
            }

            return false;
        }


        private void AddEntry(QuestLogEntry entry)
        {
            _quests.Add(entry);            
        }


        public QuestLogEntry GetQuestLogEntry(long questId)
        {
            return _quests.FirstOrDefault(x => x.Quest.QuestId == questId);
        }

        /// <summary>
        /// Fetches a list of entries that are currently active with a state of currently active.
        /// 
        /// Quests which have been finished or are not currently available are not included.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QuestLogEntry> GetActiveQuestLogEntries()
        {
            return _quests.Where(x => x.State == QuestState.InProgress).ToList();
        }

        /// <summary>
        /// Fetches a list of entries that are considered complete within the game. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QuestLogEntry> GetFinishedQuestLogEntries()
        {
            return _quests.Where(x => x.State == QuestState.Finished).ToList();
        }

        public IEnumerator<QuestLogEntry> GetEnumerator()
        {
            return _quests.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}

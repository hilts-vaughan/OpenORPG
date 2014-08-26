using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models;
using Server.Infrastructure.Quests;

namespace Server.Game.Quests
{
    /// <summary>
    /// A quest entry contains some basic information regarding a quest, steps, and how to complete it.
    /// </summary>
    public class QuestEntry
    {
        private Quest _quest;
        private UserQuestInfo _questInfo;

        public QuestEntry(Quest quest, UserQuestInfo questInfo)
        {
            _quest = quest;
            _questInfo = questInfo;

        }

        public Quest Quest
        {
            get { return _quest; }
        }

        /// <summary>
        /// Returns the current step of the quest this user is on in the entry.
        /// If the progress cannot be determined, null is returned.
        /// </summary>
        public QuestStep CurrentStep
        {
            get
            {
                if (_quest.Steps.Count - 1 > _questInfo.QuestProgress)
                    return null;

                return _quest.Steps[_questInfo.QuestProgress];
            }
        }



    }
}

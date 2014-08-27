using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using Server.Game.Database.Models;
using Server.Infrastructure.Quests;
using Server.Infrastructure.Quests.Requirements;

namespace Server.Game.Quests
{
    /// <summary>
    /// A quest entry contains some basic information regarding a quest, steps, and how to complete it.
    /// </summary>
    public class QuestLogEntry
    {

        private readonly List<int> _requirementProgress = new List<int>();

        public List<int> GetProgress()
        {
            return _requirementProgress;
        } 

        public delegate void QuestLogEntryEvent(QuestLogEntry sender);

        public event QuestLogEntryEvent ProgressAdvanced;

        protected virtual void OnProgressAdvanced()
        {
            QuestLogEntryEvent handler = ProgressAdvanced;
            if (handler != null) handler(this);
        }


        private Quest _quest;
        private UserQuestInfo _questInfo;

        public QuestLogEntry(Quest quest, UserQuestInfo questInfo)
        {
            _quest = quest;
            _questInfo = questInfo;

            // Reset stuff with a progress of zero
            for (int i = 0; i < CurrentStep.Requirements.Count; i++)
                _requirementProgress.Add(0);

            for (int i = 0; i < _questInfo.RequirementProgress.Count - 1; i++)
                _requirementProgress[i] = _questInfo.RequirementProgress[i].Progress;

         
       
        }

        public void AdvanceStep()
        {
            // Increment the progress flag and let subscribers know about this
            _questInfo.QuestProgress++;

            // Reset the requirement progress flags
            _requirementProgress.Clear();
           
            // Reset stuff with a progress of zero
            for(int i = 0; i < CurrentStep.Requirements.Count; i++)
                _requirementProgress.Add(0);

            OnProgressAdvanced();
        }



        public QuestState State
        {
            get { return _questInfo.State; }
            set { _questInfo.State = value; }
        }

        public Quest Quest
        {
            get { return _quest; }
        }

        public UserQuestInfo QuestInfo
        {
            get { return _questInfo; }
        }

        /// <summary>
        /// Returns the current step of the quest this user is on in the entry.
        /// If the progress cannot be determined, null is returned.
        /// </summary>
        public QuestStep CurrentStep
        {
            get
            {
                if (_questInfo.QuestProgress > _quest.Steps.Count - 1)
                    return null;

                return _quest.Steps[_questInfo.QuestProgress];
            }
        }



    }
}

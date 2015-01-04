using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models.Quests;
using Server.Game.Entities;
using Server.Infrastructure.Quests;

namespace Server.Infrastructure.Scripting.Quests
{

    /// <summary>
    /// A script that is designed to run executable logic on behalf of a <see cref="Quest"/>.
    /// This allows providing easy to use hooks for logic throughout the entire Quest lifecycle.
    /// </summary>
    public class QuestScript
    {

        private Quest _quest;

        public void Init(Quest quest)
        {
            _quest = quest;
        }

        /// <summary>
        /// The skill that is attached to this
        /// </summary>
        public Quest Quest
        {
            get { return _quest; }
        }


        /// <summary>
        /// When a quest has been started, this method will be invoked allowing custom logic to be run.
        /// </summary>
        /// <param name="user">This parameter will contain the player that just started the quest.</param>
        public virtual void OnQuestStarted(Player user)
        {
            
        }

        public virtual void OnQuestEnded(Player user)
        {
            
        }

        /// <summary>
        /// When a <see cref="QuestStep"/> is completed, this method will be invoked allowing custom logic to run.
        /// This method will be run once for each quest step in the quest sequence that is completed and only once.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="step"></param>
        public virtual void OnQuestStepCompleted(Player user, QuestStep step)
        {
            
        }


    }
}

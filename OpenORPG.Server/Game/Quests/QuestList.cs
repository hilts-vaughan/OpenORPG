using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models;
using Server.Game.Entities;
using Server.Infrastructure.Quests;

namespace Server.Game.Quests
{
    /// <summary>
    /// A quest list contains a listing of quest information for a particular user, <see cref="Player"/>.
    /// </summary>
    public class QuestList : IEnumerable<UserQuestInfo>
    {

        public delegate void QuestListEvent(UserQuestInfo questInfo);

        public event QuestListEvent QuestAdded;

        protected virtual void OnQuestAdded(UserQuestInfo questinfo)
        {
            QuestListEvent handler = QuestAdded;
            if (handler != null) handler(questinfo);
        }


        private List<UserQuestInfo> _quests = new List<UserQuestInfo>();


        public bool AddQuest(Quest quest)
        {
            return true;
        }


        public IEnumerator<UserQuestInfo> GetEnumerator()
        {
            return _quests.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

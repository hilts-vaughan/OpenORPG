using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Quests.Rewards;

namespace Server.Infrastructure.Quests
{
    /// <summary>
    /// A reward set is a set of rewards that a particular quest might offer to a player upon completion. 
    /// </summary>
    public class QuestRewardSet
    {

        public List<IQuestReward> Rewards { get; private set; }

        public QuestRewardSet(IEnumerable<IQuestReward> rewards)
        {
            Rewards.AddRange(rewards);
        }




    }
}

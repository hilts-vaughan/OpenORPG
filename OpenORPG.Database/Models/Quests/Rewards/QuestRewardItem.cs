using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenORPG.Database.Models.Quests.Rewards
{
    [Table("quest_reward_item")]
    public class QuestRewardItem : QuestReward
    {
        public int ItemId { get; set; }

        public byte Amount { get; set; }

    }
}

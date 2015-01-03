using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenORPG.Database.Models.Quests.Rewards
{
    [Table("quest_reward_experience")]
    public class QuestRewardExperience : QuestReward
    {

 
        public int Amount { get; set; }

        public override string DisplayString
        {
            get { return string.Format("EXP: {0}", Amount); }
        }
    }
}

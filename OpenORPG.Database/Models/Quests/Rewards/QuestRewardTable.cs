using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenORPG.Database.Models.Quests.Rewards
{

    [Table("quest_rewards")]
    public abstract class QuestReward
    {


        public int QuestRewardId { get; set; }

        [NotMapped] 
        public abstract string DisplayString { get; }


    }
}

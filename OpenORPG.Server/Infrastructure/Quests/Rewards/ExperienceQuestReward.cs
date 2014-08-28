using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Models.Quests.Rewards;
using Server.Game.Entities;

namespace Server.Infrastructure.Quests.Rewards
{
    /// <summary>
    /// A concrete implementation of a quest reward that gives out experience points.
    /// </summary>
    public class ExperienceQuestReward : QuestRewardGame<QuestRewardExperience>, IQuestReward
    {
        public ExperienceQuestReward(QuestRewardExperience rewardInfo)
            : base(rewardInfo)
        {
            RewardInfo = rewardInfo;
        }

        public bool CanGive(Player player)
        {
            return true;
        }

        public void Give(Player player)
        {
            player.Experience += RewardInfo.Amount;
        }

        public QuestRewardExperience RewardInfo { get; set; }
    }
}

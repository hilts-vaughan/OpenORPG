using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Infrastructure.Quests.Rewards
{
    /// <summary>
    /// A concrete implementation of a quest reward that gives out experience points.
    /// </summary>
    public class ExperienceQuestReward : IQuestReward
    {
        public int ExperienceReward { get; set; }

        public ExperienceQuestReward(int experienceReward)
        {
            ExperienceReward = experienceReward;
        }

        public bool CanGive(Player player)
        {
            return true;
        }

        public void Give(Player player)
        {
            player.Experience += ExperienceReward;
        }
    }
}

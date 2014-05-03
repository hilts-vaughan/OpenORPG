using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Infrastructure.Quests.Rewards
{
   
    public interface IQuestReward
    {

        /// <summary>
        /// Verifys that this reward can be given to the player.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        bool CanGive(Player player);

        /// <summary>
        /// Gives the reward to the player.
        /// </summary>
        /// <param name="player"></param>
        void Give(Player player);


    }
}

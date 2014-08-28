using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.DAL;
using OpenORPG.Database.Models.Quests.Rewards;
using Server.Game.Database;
using Server.Game.Entities;
using Server.Game.Items;

namespace Server.Infrastructure.Quests.Rewards
{
    /// <summary>
    /// A reward that gives a user an item for the completion of the quest
    /// </summary>
    public class ItemQuestReward : QuestRewardGame<QuestRewardItem>, IQuestReward
    {
        public ItemQuestReward(QuestRewardItem rewardInfo)
            : base(rewardInfo)
        {
            RewardInfo = rewardInfo;
        }

        public bool CanGive(Player player)
        {
            // The player must have room for the item
            return !player.Backpack.IsFull;
        }

        public void Give(Player player)
        {
            using (var context = new GameDatabaseContext())
            {
                var repo = new ItemRepository(context);
                var template = repo.Get(RewardInfo.ItemId);

                var item = ItemFactory.CreateItem(template);
                player.AddToBackpack(item, RewardInfo.Amount);
            }          
        }

        public QuestRewardItem RewardInfo { get; set; }
    }
}

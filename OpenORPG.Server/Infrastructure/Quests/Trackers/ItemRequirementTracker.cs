using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Models.Quests;
using Server.Game.Entities;
using Server.Game.Quests;
using Server.Game.Zones;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Quests.Requirements;
using Server.Infrastructure.World;

namespace Server.Infrastructure.Quests.Trackers
{
    public class ItemRequirementTracker : QuestRequirementTracker<QuestItemRequirementTable>
    {
        public ItemRequirementTracker(Zone zone)
            : base(zone)
        {
        }


        public override void OnEntityAdded(Entity entity)
        {
            if (entity is Player)
                OnPlayerAdded(entity as Player);
        }

        private void OnPlayerAdded(Player player)
        {
            player.BackpackChanged += PlayerOnBackpackChanged;
        }

        private void OnPlayerRemoved(Player player)
        {
            player.BackpackChanged -= PlayerOnBackpackChanged;
        }


        private void PlayerOnBackpackChanged(Player player)
        {
            foreach (var activeRequirement in GetQuestEntryWithRequirementType<QuestHasItemRequirement>(player))
            {
                // Fetch our results from our tuple
                var entry = activeRequirement.Item1;
                var requirement = activeRequirement.Item2;
                var i = activeRequirement.Item3;

                var itemCount = player.Backpack.CountItems(requirement.RequirementInfo.ItemId);
                var newValue = entry.SetProgress(i, (int) itemCount);
                OnProgressChanged(player, entry, i, newValue);
            }

        }

        public override void OnEntityRemoved(Entity entity)
        {
            if (entity is Player)
                OnPlayerRemoved(entity as Player);
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Models.Quests;
using Server.Game.Zones;
using Server.Infrastructure.World;

namespace Server.Infrastructure.Quests.Trackers
{
    /// <summary>
    /// Describes a class that tracks certain quest requirements in the context of a zone.
    /// </summary>
    public abstract class QuestRequirementTracker<T> where T : QuestRequirement
    {

        protected Zone Zone;

        protected QuestRequirementTracker(Zone zone)
        {
            Zone = zone;
        }

        public abstract void OnEntityAdded(Entity entity);
        public abstract void OnEntityRemoved(Entity entity);
                            
        T RequirementInfo { get; set; }

    }
}

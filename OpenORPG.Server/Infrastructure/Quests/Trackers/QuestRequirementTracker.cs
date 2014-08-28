using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Models.Quests;
using Server.Game.Entities;
using Server.Game.Quests;
using Server.Game.Zones;
using Server.Infrastructure.World;

namespace Server.Infrastructure.Quests.Trackers
{
    /// <summary>
    /// Describes a class that tracks certain quest requirements in the context of a zone.
    /// </summary>
    public abstract class QuestRequirementTracker<T> : IQuestRequirementTracker
    {

        protected Zone Zone;

        protected QuestRequirementTracker(Zone zone)
        {
            Zone = zone;
        }

        
                            
        T RequirementInfo { get; set; }

        public event QuestRequirementTrackerEvent ProgressChanged;

        protected virtual void OnProgressChanged(Player player, QuestLogEntry entry, int index, int progress)
        {
            QuestRequirementTrackerEvent handler = ProgressChanged;
            if (handler != null) handler(player, entry, index, progress);
        }

        public abstract void OnEntityAdded(Entity entity);

        public abstract void OnEntityRemoved(Entity entity);


    }

    public delegate void QuestRequirementTrackerEvent(Player player, QuestLogEntry entry, int index, int progress);

    public interface IQuestRequirementTracker
    {

        event QuestRequirementTrackerEvent ProgressChanged;
        
        void OnEntityAdded(Entity entity);
        
        void OnEntityRemoved(Entity entity);



    }

}

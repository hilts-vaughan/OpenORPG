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

        /// <summary>
        /// Examines the quest log entry of a user and the associated requirements and determines
        /// what requirements are active of a certain type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>An iterator version of the current tupled entry</returns>
        protected IEnumerable<Tuple<QuestLogEntry, T, int>> GetQuestEntryWithRequirementType<T>(Player player)
            where T : class
        {
            foreach (var entry in player.QuestLog.GetActiveQuestLogEntries())
            {
                var step = entry.CurrentStep;

                if (step != null)
                {
                    for (int i = 0; i < step.Requirements.Count; i++)
                    {
                        var requirement = step.Requirements[i];
                        var typedRequirement = requirement as T;

                        if (typedRequirement != null)
                            yield return Tuple.Create(entry, typedRequirement, i);
                    }
                }

            }
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

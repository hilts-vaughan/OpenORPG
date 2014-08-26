using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Quests
{
    /// <summary>
    /// Represents a quest progress 
    /// </summary>
    public struct QuestProgress
    {

        /// <summary>
        /// This represents the current goal progress for a particular quest requirement.
        /// </summary>
        public readonly int Current;

        /// <summary>
        /// This represents the target goal progress for a particular quest requirement.
        /// </summary>
        public readonly int Goal;

        public QuestProgress(int current, int goal)
        {
            Current = current;
            Goal = goal;
        }

        public QuestProgress(long current, long goal)
        {
            Current = (int) current;
            Goal = (int) goal;
        }


    }
}

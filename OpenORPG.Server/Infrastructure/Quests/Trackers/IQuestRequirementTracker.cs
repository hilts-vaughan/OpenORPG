using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Quests.Trackers
{
    /// <summary>
    /// Describes a class that tracks certain quest requirement types
    /// </summary>
    public interface IQuestRequirementTracker
    {

        void RegisterEntry();

    }
}

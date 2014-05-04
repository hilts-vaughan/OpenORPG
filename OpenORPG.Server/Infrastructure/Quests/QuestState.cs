using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Quests
{
    /// <summary>
    /// This represents any possible states that a <see cref="Quest"/> can potentially be in.
    /// </summary>
    public enum QuestState
    {
        Available,
        InProgress,
        Finished

    }
}

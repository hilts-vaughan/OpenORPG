using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Quests
{
    /// <summary>
    /// Provides an interface to an object that can give out quests.
    /// </summary>
    public interface IQuestProvider
    {

        /// <summary>
        /// A list of quests that this NPC can provide
        /// </summary>
        List<Quest> Quests { get; set; } 
    }
}

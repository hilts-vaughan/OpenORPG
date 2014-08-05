using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenORPG.Database.Enums
{

    /// <summary>
    /// A <see cref="QuestType"/> is a simple enumeration type of 
    /// </summary>
    public enum QuestType
    {

        /// <summary>
        /// A normal quest will appear in a quest log as a simple entry, with data as normal.
        /// </summary>
        Normal,

        /// <summary>
        /// A story quest is one that is considered to be integral the main storyline of a game. This might change as time goes on.
        /// </summary>
        Story
    }
}

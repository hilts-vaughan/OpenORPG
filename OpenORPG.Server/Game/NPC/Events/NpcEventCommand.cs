using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.NPC.Events
{
    /// <summary>
    /// Represents an abstract NPC event that can be used to interface with the game world.
    /// </summary>
    public abstract class NpcEventCommand
    {
        protected NpcEventReceiverContext Context;

        protected NpcEventCommand(NpcEventReceiverContext context)
        {
            Context = context;
        }

        public abstract void Execute();

    }
}

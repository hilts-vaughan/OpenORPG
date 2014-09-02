using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.NPC.Events.Commands
{
    /// <summary>
    /// An event command that signals the display of event text events.
    /// </summary>
    public class EventShowTextCommand : NpcEventCommand
    {
        private string _stringTableId;

        public EventShowTextCommand(NpcEventReceiverContext context, string stringTableId) : base(context)
        {
            _stringTableId = stringTableId;
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }


    }
}

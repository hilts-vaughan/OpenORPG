using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Models.Events;

namespace Server.Game.NPC.Events
{
    /// <summary>
    /// Responsible for invoking commands.
    /// </summary>
    public class NpcEventInvoker
    {

        public void Execute(NpcEventCommand command)
        {
            command.Execute();
        }



    }
}

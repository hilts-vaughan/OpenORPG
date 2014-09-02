using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Models.Events;
using Server.Game.Entities;

namespace Server.Game.NPC.Events
{
    /// <summary>
    /// A context that is given to a command to be executed and used.
    /// </summary>
    public sealed class NpcEventReceiverContext : IReceiver
    {
        
        /// <summary>
        /// Holds a reference to the player that executed this sequence of events.
        /// </summary>
        public Player LocalPlayer { get; private set; }

        /// <summary>
        /// Holds a reference to the local Npc that is using these events.
        /// </summary>
        public Npc LocalNpc { get; private set; }

        public NpcEventReceiverContext(Player localPlayer, Npc localNpc)
        {
            LocalPlayer = localPlayer;
            LocalNpc = localNpc;
        }


    }
}

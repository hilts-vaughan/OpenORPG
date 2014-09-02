using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Items;

namespace Server.Game.NPC.Events.Commands
{
    /// <summary>
    /// An event command that gives an item to the player interacting with the event.
    /// </summary>
    public class EventGiveItemCommand : NpcEventCommand
    {
        private Item _item;

        public EventGiveItemCommand(NpcEventReceiverContext context, Item item)
            : base(context)
        {
            _item = item;
        }

        public override void Execute()
        {
            Context.LocalPlayer.Backpack.TryAddItem(_item);
        }



    }
}

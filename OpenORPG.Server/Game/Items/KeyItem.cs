using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Entities;
using Server.Game.Network.Packets.Server;

namespace Server.Game.Items
{
    /// <summary>
    /// A <see cref="KeyItem"/> serves no immediate purpose as an item. Typically, they are used in things like quests
    /// or used as status items. Sometimes, they're used for other tasks in game. At any rate, a user cannot use them
    /// to do anything.
    /// </summary>
    public class KeyItem : Item
    {
        public KeyItem(ItemTemplate itemTemplate) : base(itemTemplate)
        {
        }


        public override void UseItemOn(Character character, Character user)
        {
            var player = character as Player;

            if (player != null)
            {
                // Key items cannot be used; send a message notifying the user
                var packet = new ServerSendGameMessagePacket(GameMessage.ItemCannotBeUsed, new List<string>());
                player.Client.Send(packet);
            }

            
        }
    }
}

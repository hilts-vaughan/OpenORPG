using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Storage;

namespace Server.Game.Items.Storage
{
    /// <summary>
    /// A slot in an <see cref="ItemStorage"/> for storing various user items.
    /// </summary>
    public class ItemSlot
    {
        /// <summary>
        /// Constructs a new item slot
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public ItemSlot(Item item, long amount)
        {
            Item = item;
            Amount = amount;
        }

        /// <summary>
        /// The item that is inside of this slot
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// The amount of item in this slot
        /// </summary>
        public long Amount { get; set; }


        /// <summary>
        /// Removes a single item 
        /// </summary>
        public bool RemoveSingle()
        {
            Amount--;

            if (Amount <= 0)
                return true;
            return false;
        }


    }
}

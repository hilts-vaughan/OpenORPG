using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Items;
using Server.Game.Items.Storage;

namespace Server.Game.Storage
{
    /// <summary>
    /// An item storage is a piece of storage that can carry a particular amount of items.
    /// 
    /// This includes things like backpacks and storage banks.
    /// </summary>
    public class ItemStorage
    {
        private Dictionary<long, ItemSlot> _storage = new Dictionary<long, ItemSlot>();

        public Dictionary<long, ItemSlot> Storage
        {
            get { return _storage; }
        }

        public int Capacity { get; private set; }

        public ItemStorage()
        {
            // Sets the inital capacity
            Capacity = 25;
        }

        /// <summary>
        /// Returns the item at the given slot if available, otherwise returns null.
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        public ItemSlot GetItemInfoAt(long slotId)
        {
            if (slotId > _storage.Count - 1 || _storage.ContainsKey(slotId) == false)
                return null;

            return _storage[slotId];
        }

        /// <summary>
        /// Removes all items at the given slot
        /// </summary>
        /// <param name="slotId"></param>
        public void RemoveItemAt(long slotId)
        {
            _storage.Remove(slotId);
        }

        public void RemoveSingleAt(long slotId)
        {
            if (_storage.ContainsKey(slotId))
            {
                var slot = _storage[slotId];
                var gone = slot.RemoveSingle();

                if (gone)
                    _storage[slotId] = null;

            }

        }


        /// <summary>
        /// Moves the item from the source to the destination.
        /// </summary>
        /// <param name="sourceSlot"></param>
        /// <param name="destSlot"></param>
        public bool MoveTo(long sourceSlot, long destSlot)
        {
            // Don't bother if the source does not exist
            if (!_storage.ContainsKey(sourceSlot))
                return false;

            var source = _storage[sourceSlot];

            // Perform the actual move
            if (!_storage.ContainsKey(destSlot) && destSlot < Capacity - 1)
            {
                _storage.Add(destSlot, source);
                _storage.Remove(sourceSlot);
                return true;
            }

            // Otherwise, the slot was occupied so there's no sense in attempting to replace them
            return false;
        }


        /// <summary>
        /// Attempts to add the item to the next free slot in the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Returns true if a success, otherwise returns false.</returns>
        public bool TryAddItem(Item item)
        {
            return TryAddItem(item, 1);
        }

        public bool TryAddItem(Item item, long amount)
        {
            var freeSlot = GetNextFreeSlotId();
            if (freeSlot > -1)
            {
                _storage.Add(freeSlot, new ItemSlot(item, amount));
                return true;
            }

            return false;
        }

        public bool IsFull
        {
            get
            {
                for (int i = 0; i < Capacity; i++)
                {
                    if (!_storage.ContainsKey(i))
                        return false;
                }
                return true;
            }
        }

        private long GetNextFreeSlotId()
        {
            for (int i = 0; i < Capacity; i++)
            {
                if (!_storage.ContainsKey(i))
                    return i;
            }
            return -1;
        }

      
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Items;

namespace Server.Game.Storage
{
    /// <summary>
    /// An item storage is a piece of storage that can carry a particular amount of items.
    /// 
    /// This includes things like backpacks and storage banks.
    /// </summary>
    public class ItemStorage : IEnumerable<Item>
    {
        private List<Item> _storage = new List<Item>();

        public int Capacity { get; set; }

        public ItemStorage()
        {

        }

        /// <summary>
        /// Returns the item at the given slot if available, otherwise returns null.
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        public Item GetItemAt(long slotId)
        {
            if (slotId > _storage.Count - 1)
                return null;

            return _storage[(int)slotId];
        }

        public void RemoveItemAt(long slotId)
        {
            if (slotId > _storage.Count - 1)
                return;

            _storage[(int)slotId] = null;
        }

        public void AddItem(Item item)
        {
            _storage.Add(item);
        }

        //TODO: Implement this properly
        public bool IsFull
        {
            get { return false; }
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

    }
}

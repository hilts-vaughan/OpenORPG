using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Storage
{
    /// <summary>
    /// An item storage is a piece of storage that can carry a particular amount of items.
    /// </summary>
    public class ItemStorage : IEnumerable<Item>
    {
        private List<Item> _storage = new List<Item>();
 
        public int Capacity { get; set; }

        public ItemStorage()
        {
            
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

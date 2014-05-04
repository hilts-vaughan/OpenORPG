using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Entities
{
    /// <summary>
    /// A simple structure containing information
    /// </summary>
    public struct NpcInfo
    {
        /// <summary>
        /// A list of quest Ids that this Npc can distribute to players. 
        /// </summary>
        public List<long> QuestIds { get; set; }

        /// <summary>
        /// Tne Id of the shop this Npc will run. This will be 0 if the Npc does not run a shop.
        /// Otherwise, this is the Id of the shop.
        /// </summary>
        public long ShopId { get; set; }

        /// <summary>
        /// The name this Npc will represent
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sprite this Npc will use
        /// </summary>
        public string Sprite { get; set; }
    }
}

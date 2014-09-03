using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Database.Models.Quests;

namespace Server.Game.Database.Models
{
    [Table("npc_template")]
    public class NpcTemplate : IContentTemplate
    {

        [Key, Required]
        public int Id { get; set; }

        public NpcTemplate()
        {
            Quests = new List<QuestTemplate>();
        }


        /// <summary>
        /// A quest that this NPC can have
        /// </summary
        public virtual ICollection<QuestTemplate> Quests { get; set; }

        /// <summary>
        /// Tne Id of the shop this Npc will run. This will be 0 if the Npc does not run a shop.
        /// Otherwise, this is the Id of the shop.
        /// </summary>
        public long ShopId { get; set; }

        /// <summary>
        /// The name this Npc will represent
        /// </summary>
        public string Name { get; set; }

        public string VirtualCategory { get; set; }

        /// <summary>
        /// The sprite this Npc will use
        /// </summary>
        public string Sprite { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

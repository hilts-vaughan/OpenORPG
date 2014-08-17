using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models.ContentTemplates;

namespace OpenORPG.Database.Models.ContentTemplates
{
    public class StatusEffectTemplate : IContentTemplate
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VirtualCategory { get; set; }

        // Stuff that isn't part of the interface is deposited below

        /// <summary>
        /// The amount of time it will take for a status effect to 'tick'. A script will be invoked accordingly 
        /// </summary>
        public int TickTime { get; set; }
       

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Models.ContentTemplates;
using Server.Game.Entities;

namespace Server.Game.Combat.Effects
{
    /// <summary>
    /// A status effect modifies the behavior of a <see cref="Character"/> in the game world by modifying certain
    /// properties throughout the world.
    /// </summary>
    public abstract class StatusEffect
    {
        public StatusEffectTemplate StatusTemplate { get; private set; }

        /// <summary>
        /// The duration of the status effect, in seconds. The status effect will automatically be removed
        /// after this amount of time. 
        /// </summary>
        public long Duration { get; set; }

        protected StatusEffect(StatusEffectTemplate statusTemplate, long duration)
        {
            StatusTemplate = statusTemplate;
            Duration = duration;
        }



    }
}

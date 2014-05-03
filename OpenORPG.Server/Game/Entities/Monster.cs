using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models.ContentTemplates;

namespace Server.Game.Entities
{
    /// <summary>
    /// An NPC is a <see cref="Character"/> that is not controlled directly by a player in a simulation.
    /// They have similar characteristics but have their movement and systems determined by the system.
    /// </summary>
    public class Monster : Character
    {
        public Monster(MonsterTemplate monsterTemplate)
            : base(monsterTemplate.Sprite)
        {
            // Copy everything we need to into the object
            Name = monsterTemplate.Name;

        }

        public int OriginSpawn { get; set; }


    }
}

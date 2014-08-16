using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Combat.Payloads;
using Server.Game.Entities;

namespace Server.Game.Combat
{
    /// <summary>
    /// An attack payload is an attack that is ready to be applied to any entity.
    /// 
    /// Any combat damage should be applied via a payload so subsystems can track this.
    /// </summary>
    public class DamagePayload : Payload
    {
        public long DamageInflicted { get; set; }

        public DamagePayload(Character aggressor, int damage)
            : base(aggressor)
        {
            DamageInflicted = damage;
        }

        public override void Apply(Character victim)
        {
            victim.CharacterStats[(int)StatTypes.Hitpoints].CurrentValue = victim.CharacterStats[StatTypes.Hitpoints].CurrentValue - DamageInflicted;
        }



  


    }
}

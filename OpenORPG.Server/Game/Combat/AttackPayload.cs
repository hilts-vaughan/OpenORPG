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
    /// A payload does all the computations for damage and application of status effects.
    /// 
    /// Any combat damage should be applied via a payload so subsystems can track this.
    /// </summary>
    public class AttackPayload : Payload
    {
        public AttackPayload(Character aggressor) : base(aggressor)
        {
        }

        public override void Apply(Character victim)
        {
            // Set the victims hit points directly to zero
            victim.CharacterStats[(int) StatTypes.Hitpoints].CurrentValue -= 1;
        }



    }
}

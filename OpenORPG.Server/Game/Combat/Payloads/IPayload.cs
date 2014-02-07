using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Game.Combat.Payloads
{
    /// <summary>
    /// A payload is a result of a combat action that is applied to a <see cref="Character"/>
    /// </summary>
    public abstract class Payload
    {
        protected Payload(Character aggressor)
        {
            Aggressor = aggressor;
        }

        /// <summary>
        /// The aggressor is the initiator of the attack payload.
        /// </summary>
        public Character Aggressor { get; private set; }

        /// <summary>
        /// Applies the payload to the <see cref="Character"/> that needs to be damaged.
        /// </summary>
        /// <param name="victim">The character that should be damaged by the payload</param>
        public abstract void Apply(Character victim);

    }
}

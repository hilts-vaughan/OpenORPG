using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Game.Combat.Payloads
{
    public class HealingPayload: Payload
    {
        public HealingPayload(Character aggressor) : base(aggressor)
        {
        }

        public override void Apply(Character victim)
        {
            throw new NotImplementedException();
        }
    }
}

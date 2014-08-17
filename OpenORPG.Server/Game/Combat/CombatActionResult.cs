using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Combat.Actions;

namespace Server.Game.Combat
{
    
    /// <summary>
    /// Provides information about the result and outcome of an <see cref="CombatAction"/> that
    /// was applied.
    /// </summary>
    public struct CombatActionResult
    {
        public long TargetId { get; set; }
        public long Damage { get; set; }

        public CombatActionResult(long targetId, long damage) : this()
        {
            TargetId = targetId;
            Damage = damage;
        }

    }
}

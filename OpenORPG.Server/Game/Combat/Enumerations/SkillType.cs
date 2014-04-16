using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Combat
{
    /// <summary>
    /// Defines a skill type
    /// </summary>
    public enum SkillType
    {
        /// <summary>
        /// This is a standard damage dealing skill type
        /// </summary>
        Damage,
        Healing,
        Status,

        /// <summary>
        /// Summoning skills spawn other entities
        /// </summary>
        Summoning,

        /// <summary>
        /// Special skills have their logic executed via scripts. This is common for skills with special rules.
        /// </summary>
        Special

    }
}

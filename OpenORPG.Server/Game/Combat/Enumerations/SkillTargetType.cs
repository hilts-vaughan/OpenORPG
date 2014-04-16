using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Combat
{
    [Flags]
    public enum SkillTargetType
    {
        /// <summary>
        /// The player flags is any player, regardless of being an ally or not
        /// </summary>
        Players = 1,

        /// <summary>
        /// The ally flag is for any ally member
        /// </summary>
        Ally = 2,

        /// <summary>
        /// The enemy flag is for any enemy
        /// </summary>
        Enemy = 4,
    }


}

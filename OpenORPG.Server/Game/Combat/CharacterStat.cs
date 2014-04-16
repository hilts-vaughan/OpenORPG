using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Game.Combat
{
    /// <summary>
    /// A character stat is a vital part of the makeup of the <see cref="Character"/>.
    /// These usually contain information like maximum and minimums, along with any modifiers.
    /// </summary>
    public struct CharacterStat
    {
        public int MaximumValue { get; set; }
        public int CurrentValue { get; set; }

        public CharacterStat(int maximumValue, int currentValue) : this()
        {
            MaximumValue = maximumValue;
            CurrentValue = currentValue;
        }
    }
}

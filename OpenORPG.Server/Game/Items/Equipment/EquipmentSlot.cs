using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Game.Items.Equipment
{
    /// <summary>
    /// An enumeration of equipment slots that are available for <see cref="Character"/>s.
    /// These are flags as combining them is possible
    /// </summary>
    [Flags]
    public enum EquipmentSlot
    {
        Weapon,
        Head,
        Body,
        Back,
        Neck,
        Hands,
        Waist,
        Ear,
        Ring,
        Legs,
        Feet
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Combat;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Entities;

namespace Server.Game.Items.Equipment
{
    /// <summary>
    /// A piece of equipment
    /// </summary>
    public class Equipment : Item
    {
        private static readonly int NumberOfStats;

        static Equipment()
        {
            NumberOfStats = Enum.GetNames(typeof(StatTypes)).Length;
        }


        public Equipment(ItemTemplate itemTemplate) : base(itemTemplate)
        {

        }

        /// <summary>
        /// The slot that this piece of equipment can fit into
        /// </summary>
        public EquipmentSlot Slot
        {
            get { return ItemTemplate.EquipmentSlot; }
        }

        /// <summary>
        /// Fetches and returns all the modifications to stats this equipment provides.
        /// </summary>
        /// <returns>Returns an array of <see cref="CharacterStat"/> representing the values.</returns>
        public CharacterStat[] GetEquipmentModifierStats()
        {
            var stats = new CharacterStat[NumberOfStats];

            stats[(int) StatTypes.Strength].CurrentValue = ItemTemplate.StrengthModifier;
            stats[(int)StatTypes.Dexterity].CurrentValue = ItemTemplate.DexterityModifier;
            stats[(int)StatTypes.Intelligence].CurrentValue = ItemTemplate.IntelligenceModifier;
            stats[(int)StatTypes.Hitpoints].CurrentValue = ItemTemplate.HitpointsModifier;
            stats[(int)StatTypes.Luck].CurrentValue = ItemTemplate.LuckModifier;
            stats[(int)StatTypes.Vitality].CurrentValue = ItemTemplate.VitalityModifier;

            return stats;
        }



    }
}

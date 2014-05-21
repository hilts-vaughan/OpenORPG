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
        public CharacterStatCollection GetEquipmentModifierStats()
        {
            var stats = new CharacterStatCollection();

            stats[StatTypes.Strength].CurrentValue = ItemTemplate.StrengthModifier;
            stats[StatTypes.Dexterity].CurrentValue = ItemTemplate.DexterityModifier;
            stats[StatTypes.Intelligence].CurrentValue = ItemTemplate.IntelligenceModifier;
            stats[StatTypes.Hitpoints].CurrentValue = ItemTemplate.HitpointsModifier;
            stats[StatTypes.Luck].CurrentValue = ItemTemplate.LuckModifier;
            stats[StatTypes.Vitality].CurrentValue = ItemTemplate.VitalityModifier;

            return stats;
        }



    }
}

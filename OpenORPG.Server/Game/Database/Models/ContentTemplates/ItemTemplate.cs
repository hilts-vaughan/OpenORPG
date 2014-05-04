using System.Collections;
using System.Collections.Generic;
using Inspire.Shared.Models.Enums;
using Server.Game.Combat;
using Server.Game.Entities;
using Server.Game.Items.Equipment;

namespace Server.Game.Database.Models.ContentTemplates
{
    /// <summary>
    ///     An item template
    /// </summary>
    public class ItemTemplate : IContentTemplate
    {
        public ItemTemplate(long id, string name, string description, ItemType type, int price, bool consumed,
                            int useSpeed)
        {
            Id = id;
            Name = name;
            Description = description;
            Type = type;
            Price = price;
            Consumed = consumed;
            UseSpeed = useSpeed;

        }

        public ItemTemplate()
        {
            
        }

        public string Description { get; set; }
        public ItemType Type { get; set; }
        public int Price { get; set; }
        public bool Consumed { get; set; }


        /// <summary>
        /// The amount of time it takes for this item to be used (millisecondss)
        /// </summary>
        public int UseSpeed { get; set; }

        public long Id { get; set; }
        public string Name { get; set; }
        public string VirtualCategory { get; set; }

        // Below here is equipment specific stuff, this will not be used on standard items

        // Various available stat modifiers

        public long StrengthModifier { get; set; }
        public long DexterityModifier { get; set; }
        public long VitalityModifier { get; set; }
        public long IntelligenceModifier { get; set; }
        public long LuckModifier { get; set; }

        public long HitpointsModifier { get; set; }

        public long DamageModifier { get; set; }
        public EquipmentSlot EquipmentSlot { get; set; }

     

    }
}
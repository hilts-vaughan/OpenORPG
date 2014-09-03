using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Inspire.Shared.Models.Enums;
using OpenORPG.Database.Enums;

namespace Server.Game.Database.Models.ContentTemplates
{
    /// <summary>
    ///     An item template
    /// </summary>
    [Table("item_templates")]
    public class ItemTemplate : IContentTemplate
    {
        public ItemTemplate(int id, string name, string description, ItemType type, int price, bool consumed,
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

        public string Notes { get; set; }

        public ItemType Type { get; set; }
        public int Price { get; set; }
        public bool Consumed { get; set; }


        /// <summary>
        /// The Id of the icon on the given spritesheet to display for this item.
        /// </summary>
        public int IconId { get; set; }

        /// <summary>
        /// The amount of time it takes for this item to be used (millisecondss)
        /// </summary>
        public int UseSpeed { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string VirtualCategory { get; set; }

        // healing related stuff
        public long RestoreHp { get; set; }
        public bool HpPercent { get; set; }

        public long RestoreMp { get; set; }
        public bool MpPercent { get; set; }

        public long RestoreTp { get; set; }
        public bool TpPercent { get; set; }


        // Below here is equipment specific stuff, this will not be used on standard items

        // Various available stat modifiers

        public long Str { get; set; }
        public long Dex { get; set; }
        public long Vit { get; set; }
        public long Int { get; set; }
        public long Lck { get; set; }

        public long Mnd { get; set; }

        public long HitpointsModifier { get; set; }

        public long Damage { get; set; }

        public int WeaponSpeed { get; set; }

        public EquipmentSlot EquipmentSlot { get; set; }


        /// <summary>
        /// A skill ID that a user might learn by using this item.
        /// </summary>
        public int LearntSkillId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
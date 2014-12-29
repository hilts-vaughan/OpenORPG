using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inspire.Shared.Models.Enums;
using Server.Game.Database.Models.ContentTemplates;

namespace Server.Game.Items
{
    /// <summary>
    /// Creates items based on their enumeration type and given template. When creating items,
    /// one should always go through this factory so that they can be created properly.
    /// </summary>
    public static class ItemFactory
    {

        /// <summary>
        /// Creates an item based on the item templates type.
        /// </summary>
        /// <param name="itemTemplate"></param>
        /// <returns></returns>
        public static Item CreateItem(ItemTemplate itemTemplate)
        {
            switch (itemTemplate.Type)
            {
                case ItemType.Equipment:
                    return CreateEquipmentItem(itemTemplate);
                case ItemType.FieldItem:
                    return CreateFieldItem(itemTemplate);
                case ItemType.Skillbook:
                    return CreateSkillbookItem(itemTemplate);
                case ItemType.KeyItem:
                    return CreateKeyItem(itemTemplate);
            }

            throw new Exception("An item with an invalid type was provided to the create factory method.");
        }

        private static Item CreateKeyItem(ItemTemplate itemTemplate)
        {
            return new KeyItem(itemTemplate);
        }

        private static Equipment.Equipment CreateEquipmentItem(ItemTemplate itemTemplate)
        {
            return new Equipment.Equipment(itemTemplate);
        }

        private static FieldItem CreateFieldItem(ItemTemplate itemTemplate)
        {
            return new FieldItem(itemTemplate);
        }

        private static SkillbookItem CreateSkillbookItem(ItemTemplate item)
        {
            return new SkillbookItem(item);
        }

    }
}

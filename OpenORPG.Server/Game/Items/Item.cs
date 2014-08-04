using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Entities;

namespace Server.Game.Items
{
    /// <summary>
    /// A game item represented in the world.
    /// </summary>
    public abstract class Item
    {
        protected ItemTemplate ItemTemplate;

        /// <summary>
        /// Returns the name of this item
        /// </summary>
        public string Name
        {
            get { return ItemTemplate.Name; }
        }

        public bool Consumable
        {
            get { return GetType() == typeof(FieldItem) || GetType() == typeof(SkillbookItem); }
        }

        public string Description
        {
            get { return ItemTemplate.Description; }
        }

        public long Id
        {
            get { return ItemTemplate.Id; }
        }

        public string Type
        {
            get { return GetType().Name.ToString(); }
        }

        public long IconId
        {
            get { return ItemTemplate.IconId; }
        }

        protected Item(ItemTemplate itemTemplate)
        {
            ItemTemplate = itemTemplate;
        }

        /// <summary>
        /// Performs an item action on a character, allowing something to be performed.
        /// </summary>
        /// <param name="character"></param>
        public abstract void UseItemOn(Character character, Character user);


    }
}

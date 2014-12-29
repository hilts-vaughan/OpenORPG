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

        public abstract bool Consumable { get; }

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
        /// <param name="user">A parameter representing the user of the item</param>
        /// <param name="target">A parameter representing the target of the item. For self use items, the target will be the user.</param>
        public abstract void UseItemOn(Character user, Character target);


    }
}

using Server.Game.Database.Models.ContentTemplates;

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

        protected Item(ItemTemplate itemTemplate)
        {
            ItemTemplate = itemTemplate;
        }


    }
}

using Server.Game.Database.Models.ContentTemplates;

namespace Server.Game.Items
{
    /// <summary>
    /// A game item
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

        protected Item(ItemTemplate itemTemplate)
        {
            ItemTemplate = itemTemplate;
        }


    }
}

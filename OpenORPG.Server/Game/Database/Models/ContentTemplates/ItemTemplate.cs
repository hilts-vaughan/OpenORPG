using Inspire.Shared.Models.Enums;

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


        public string Description { get; set; }

        public ItemType Type { get; set; }

        public int Price { get; set; }
        public bool Consumed { get; set; }

        /// <summary>
        ///     The amount of time it takes for this item to be used (millisecondss)
        /// </summary>
        public int UseSpeed { get; set; }

        public long Id { get; set; }
        public string Name { get; set; }
        public string VirtualCategory { get; set; }
    }
}
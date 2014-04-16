using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models.ContentTemplates;

namespace Server.Game.Items
{
    /// <summary>
    /// A field item is one which can be used at any time and grants an effect.
    /// </summary>
    public class FieldItem : Item
    {
        public FieldItem(ItemTemplate itemTemplate) : base(itemTemplate)
        {

        }

    }
}

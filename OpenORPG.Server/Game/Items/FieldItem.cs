using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Entities;

namespace Server.Game.Items
{
    /// <summary>
    /// A field item is one which can be used at any time and grants an effect.
    /// </summary>
    public class FieldItem : Item
    {
        public FieldItem(ItemTemplate itemTemplate)
            : base(itemTemplate)
        {

        }

        public override void UseItemOn(Character character, Character user)
        {
            // If the item can heal, apply some healing
            if (ItemTemplate.RestoreHp > 0)
                character.CharacterStats[StatTypes.Hitpoints].CurrentValue += ItemTemplate.RestoreHp;                                
        }


    }
}

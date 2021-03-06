﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Database.Models
{

    [Table("user_inventory")]
    public class UserItem
    {
     
        public int ItemEntryId { get; set; }

        public long ItemId { get; set; }
        public long ItemAmount { get; set; }

        public long SlotId { get; set; }

        /// <summary>
        /// This refers to the user that this storage belongs to
        /// </summary>
        public virtual UserHero User { get; set; }

        public UserItem(long id, long value, long slotId)
        {
            ItemId = id;
            ItemAmount = value;
            SlotId = slotId;
        }

        public UserItem()
        {
                
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Database.Models
{
    public class UserStorage
    {
     
        public int UserId { get; set; }

        public ICollection<int> ItemIds { get; set; }
        public ICollection<int> ItemValues { get; set; } 

        /// <summary>
        /// This refers to the user that this storage belongs to
        /// </summary>
        public virtual UserHero User { get; set; }

        public UserStorage()
        {
            ItemIds = new List<int>();
            ItemValues = new List<int>();
        }

    }
}

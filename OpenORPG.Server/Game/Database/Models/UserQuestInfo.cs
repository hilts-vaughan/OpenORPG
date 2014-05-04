using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Quests;

namespace Server.Game.Database.Models
{
    /// <summary>
    /// A stateful and meaningful way of storing the progress of quests in the world for heroes.
    /// </summary>
    public class UserQuestInfo
    {
        [Key]
        [Required]
        public long UserQuestInfoId { get; set; }

        public long QuestId { get; set; }

        /// <summary>
        /// This marks the current state of a quest for a user
        /// </summary>
        public QuestState State { get; set; }

        [Required]
        public virtual UserHero UserHero { get; set; }

    }
}

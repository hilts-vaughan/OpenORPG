using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;

namespace Server.Game.Database.Models
{
    /// <summary>
    /// A stateful and meaningful way of storing the progress of quests in the world for heroes.
    /// </summary>
    [Table("user_quest_info")]
    public class UserQuestInfo
    {
        public UserQuestInfo()
        {
            State = QuestState.InProgress;            
        }

        [Key]
        [Required]
        public long UserQuestInfoId { get; set; }

        public int QuestId { get; set; }

        /// <summary>
        /// This marks the current state of a quest for a user.
        /// 
        /// Since quests that are finished are possible to repeat, this flag could be a few different things.
        /// </summary>
        public QuestState State { get; set; }

        /// <summary>
        /// A simple integer used to track the amount of mobs kill in relation to the
        /// monsters requirement.
        /// 
        /// This could be eventually replaced with something a bit more generic
        /// </summary>
        [Required, DefaultValue(0)]
        public long MobsKilled { get; set; }

        /// <summary>
        /// The quest progress ID that the user has moved along to.
        /// </summary>
        public int QuestProgress { get; set; }

        [Required]
        public virtual UserHero UserHero { get; set; }

    }
}

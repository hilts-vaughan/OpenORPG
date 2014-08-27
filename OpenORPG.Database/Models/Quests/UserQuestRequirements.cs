using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models;

namespace OpenORPG.Database.Models.Quests
{
    /// <summary>
    /// 
    /// </summary>
    [Table("user_quest_info_requirements")]
    public class UserQuestRequirements
    {
        [Key]
        public int UserQuestRequirementId { get; set; }

        public int Progress { get; set; }

        public UserQuestInfo QuestInfo { get; set; }

    }
}

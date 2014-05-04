using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models.ContentTemplates;
using Server.Infrastructure.Quests.Requirements;

namespace Server.Game.Database.Models.Quests
{
    public class QuestItemRequirementTable : IQuestRequirementTable
    {
        public QuestItemRequirementTable()
        {
            ItemId = 0;
            ItemAmount = 0;
        }


        [Key, ForeignKey("Quest")]
        public long QuestTableId { get; set; }


        public virtual QuestTable Quest { get; set; }

        [Required]
        public long ItemId { get; set; }

        [Required]
        public long ItemAmount { get; set; }

 


    }
}

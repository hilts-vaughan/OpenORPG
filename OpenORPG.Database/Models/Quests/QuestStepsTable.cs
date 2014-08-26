using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models.Quests;

namespace OpenORPG.Database.Models.Quests

{
    [Table("quest_steps")]
    public class QuestStepsTable
    {

        public QuestStepsTable()
        {
            Requirements = new List<QuestRequirement>();
        }


        public int QuestStepsTableId { get; set; }

        public virtual QuestTemplate Quest { get; set; }

        public virtual ICollection<QuestRequirement> Requirements { get; set; } 

    }
}

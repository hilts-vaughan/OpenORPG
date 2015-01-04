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

        /// <summary>
        /// This is an internally used name which is used to display in the editors.
        /// It doesn't actually need to be used in the game but helps classify things.
        /// </summary>
        public string Name { get; set; }

        public virtual List<QuestRequirement> Requirements { get; set; } 

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models.Quests;

namespace OpenORPG.Database.Models.Quests
{
    public class QuestStepsTable
    {


        public int QuestStepsTableId { get; set; }

        public virtual QuestTemplate Quest { get; set; }

        public virtual ICollection<QuestRequirement> Requirements { get; set; } 

    }
}

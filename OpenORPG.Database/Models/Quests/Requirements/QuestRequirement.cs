using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenORPG.Database.Models.Quests
{
    [Table("quest_requirements")]
    public abstract class QuestRequirement
    {
        
        public virtual QuestStepsTable QuestStep { get; set; }

        public int QuestRequirementId { get; set; }

        [NotMapped]
        public abstract string DisplayString { get; }

    }
}

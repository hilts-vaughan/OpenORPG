using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models.ContentTemplates;

namespace Server.Game.Database.Models.Quests
{
    public class QuestTable
    {
        public QuestTable()
        {
      
        }

        /// <summary>
        ///  A unique ID for this quest
        /// </summary>

        public long QuestTableId { get; set; }



        public string Name { get; set; }


        /// <summary>
        /// A quick description of the quest
        /// </summary>
        public string Description { get; set; }

        public bool CanRepeat { get; set; }

        public long RewardExp { get; set; }

        public long RewardCurrency { get; set; }

        public virtual ICollection<ItemTemplate> RewardItems { get; set; }

        public virtual QuestItemRequirementTable EndItemRequirements { get; set; }


        public virtual QuestMonsterRequirementTable EndMonsterRequirements { get; set; }

    }
}

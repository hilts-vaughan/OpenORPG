using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using OpenORPG.Database.Models.Quests;
using OpenORPG.Database.Models.Quests.Rewards;
using Server.Game.Database.Models.ContentTemplates;

namespace Server.Game.Database.Models.Quests
{
    [Table("quest_template")]
    public class QuestTemplate
    {
        public QuestTemplate()
        {
      
        }

        /// <summary>
        ///  A unique ID for this quest
        /// </summary>

        public int QuestTemplateId { get; set; }



        public string Name { get; set; }


        /// <summary>
        /// A quick description of the quest
        /// </summary>
        public string Description { get; set; }

        public bool CanRepeat { get; set; }

        public int RewardExp { get; set; }

        public long RewardCurrency { get; set; }

        public QuestType QuestType { get; set; }

        public virtual ICollection<ItemTemplate> RewardItems { get; set; }

        public virtual ICollection<QuestStepsTable> QuestSteps { get; set; }

        public virtual ICollection<QuestReward> Rewards { get; set; } 

        public virtual NpcTemplate QuestStarter { get; set; }


     

    }
}

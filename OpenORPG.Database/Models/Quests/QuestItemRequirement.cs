using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenORPG.Database.Models.Quests
{
    [Table("quest_requirements_item")]
    public class QuestItemRequirementTable : QuestRequirement
    {
        public QuestItemRequirementTable()
        {
            ItemId = 0;
            ItemAmount = 0;
        }


        [Required]
        public long ItemId { get; set; }

        [Required]
        public long ItemAmount { get; set; }

 


    }
}

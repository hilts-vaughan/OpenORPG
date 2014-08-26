using System.ComponentModel.DataAnnotations;

namespace OpenORPG.Database.Models.Quests
{
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

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


        public override string DisplayString
        {
            get { return string.Format("Collect {0} of ID {1}", ItemAmount, ItemId); }
        }
    }
}

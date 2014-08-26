using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Database.Models
{
    [Table("user_skills")]
    public class UserSkill
    {
     
        public long UserSkillId { get; set; }

        public long SkillId { get; set; }
        public virtual UserHero User { get; set; }

        public UserSkill()
        {
            
        }

        public UserSkill(long skillId)
        {
            SkillId = skillId;
        }
    }
}

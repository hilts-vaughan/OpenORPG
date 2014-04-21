using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Database.Models
{
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

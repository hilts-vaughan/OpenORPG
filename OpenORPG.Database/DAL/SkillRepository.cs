using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database;
using Server.Game.Database.Models.ContentTemplates;

namespace OpenORPG.Database.DAL
{
    public class SkillRepository : DatabaseRepository<SkillTemplate>
    {
        public SkillRepository(GameDatabaseContext context)
            : base(context)
        {

        }

      
    }
}

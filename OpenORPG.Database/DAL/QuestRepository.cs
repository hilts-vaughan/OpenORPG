using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models.Quests;

namespace OpenORPG.Database.DAL
{
    public class QuestRepository : DatabaseRepository<QuestTable>
    {
        public QuestRepository(DbContext context) : base(context)
        {

        }
    }
}

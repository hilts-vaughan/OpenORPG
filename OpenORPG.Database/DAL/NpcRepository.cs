using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models;

namespace OpenORPG.Database.DAL
{
    public class NpcRepository : DatabaseRepository<NpcTemplate>
    {
        public NpcRepository(DbContext context) : base(context)
        {
        }
    }
}

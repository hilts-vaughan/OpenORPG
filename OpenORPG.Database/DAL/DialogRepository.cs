using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Models.ContentTemplates;
using Server.Game.Database;

namespace OpenORPG.Database.DAL
{
    public class DialogRepository : DatabaseRepository<DialogTemplate>
    {
        public DialogRepository(GameDatabaseContext context) : base(context)
        {
        }

    }
}

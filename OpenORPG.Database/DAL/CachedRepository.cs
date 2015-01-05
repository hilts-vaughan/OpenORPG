using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database;

namespace OpenORPG.Database.DAL
{
    /// <summary>
    /// Uses a REDIS cached backend
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CachedRepository<T> : DatabaseRepository<T> where T : class
    {
        public CachedRepository(GameDatabaseContext context) : base(context)
        {

        }

        public override T Get(int id)
        {
            

            // fall back to getting from the database
            return base.Get(id);
        }
    }
}

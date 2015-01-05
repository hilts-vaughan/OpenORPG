using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database;
using Server.Game.Database.Models.ContentTemplates;

namespace OpenORPG.Database.DAL
{
    /// <summary>
    /// A database repository is an abstraction around fetching objects from a given database to encourage strongly typed
    /// queries.
    /// </summary>
    /// <typeparam name="T">The type of entity that is being looked up within the database</typeparam>
    public class DatabaseRepository<T> : IRepository<T> where T : class
    {
        protected GameDatabaseContext Context;

        public DatabaseRepository(GameDatabaseContext context)
        {
            if(context == null)
                throw new NullReferenceException("A context cannot be null; require a valid reference to make queries");

            Context = context;
        }


        public virtual T Get(int id)
        {
            return Context.Set<T>().Find(id);
        }

        public virtual List<T> GetAll()
        {
            var set = Context.Set<T>().ToList();
            
            PostGet(set);

            return set.ToList();
        }

        public virtual T Add(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();
            return entity;
        }

        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
        }

        public virtual T Update(T entity, int key)
        {

            if (entity == null)
                return null;

            T existing = Context.Set<T>().Find(key);
            if (existing != null)
            {
                Context.Entry(existing).CurrentValues.SetValues(entity);
                PostUpdate(entity, key);
                Context.SaveChanges();
            }
            return existing;
        }

        protected virtual void PostUpdate(T entity, int key)
        {
            
        }

        protected virtual void PostGet(List<T> entities)
        {

        }

    }
}

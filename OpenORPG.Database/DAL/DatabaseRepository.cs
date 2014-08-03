using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace OpenORPG.Database.DAL
{
    /// <summary>
    /// A database repository is an abstraction around fetching objects from a given database to encourage strongly typed
    /// queries.
    /// </summary>
    /// <typeparam name="T">The type of entity that is being looked up within the database</typeparam>
    public class DatabaseRepository<T> : IRepository<T> where T : class
    {
        private DbContext _context;

        public DatabaseRepository(DbContext context)
        {
            if(context == null)
                throw new NullReferenceException("A context cannot be null; require a valid reference to make queries");

            _context = context;
        }


        public virtual T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public virtual T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public virtual T Update(T entity, int key)
        {

            if (entity == null)
                return null;

            T existing = _context.Set<T>().Find(key);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
            return existing;
        }



    }
}

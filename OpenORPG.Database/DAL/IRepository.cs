using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenORPG.Database.DAL
{
    /// <summary>
    /// A generic repository that allows implementing various
    /// </summary>
    public interface IRepository<T> where T : class
    {   
        /// <summary>
        /// Adds an entity to the given repository.
        /// </summary>
        /// <param name="entity"></param>
        T Add(T entity);
        
        void Delete(T entity);
       
        T Update(T entity, int key);
       
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tornado.Shared.Dapper.Interfaces
{
    public interface ICreateAsync<TEntity>
       where TEntity : class
    {
        /// <summary>
        /// Create a new entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Task</returns>
        Task CreateAsync(TEntity entity);
    }
}

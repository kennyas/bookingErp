using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tornado.Shared.Dapper.Interfaces
{
    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IDeleteAsync<TEntity>
      where TEntity : class
    {
        /// <summary>
        /// Delete an existing entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Task</returns>
        Task DeleteAsync(TEntity entity);
    }
}

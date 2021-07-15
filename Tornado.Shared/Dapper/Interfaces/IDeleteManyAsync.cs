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
    public interface IDeleteManyAsync<TEntity>
      where TEntity : class
    {
        /// <summary>
        /// Delete a list of existing entities
        /// </summary>
        /// <param name="entities">Entity list</param>
        /// <returns>Task</returns>
        Task DeleteManyAsync(IEnumerable<TEntity> entities);
    }
}

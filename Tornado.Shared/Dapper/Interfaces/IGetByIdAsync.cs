using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tornado.Shared.Dapper.Interfaces
{
    /// <summary>
    /// Gets an entity by id.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IGetByIdAsync<TEntity>
      where TEntity : class
    {
        /// <summary>
        /// Gets an entity by id.
        /// </summary>
        /// <param name="id">Filter to find a single item</param>
        /// <returns>Entity</returns>
        Task<TEntity> GetByIdAsync(object id);
    }
}

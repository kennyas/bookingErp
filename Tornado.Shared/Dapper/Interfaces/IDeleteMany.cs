using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tornado.Shared.Dapper.Interfaces
{
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IDeleteMany<TEntity>
      where TEntity : class
    {
        /// <summary>
        /// Delete a list of existing entities
        /// </summary>
        /// <param name="entities">Entity list</param>
        void DeleteMany(IEnumerable<TEntity> entities);
    }
}

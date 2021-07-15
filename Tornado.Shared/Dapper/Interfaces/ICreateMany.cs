using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.Dapper.Interfaces
{
    public interface ICreateMany<TEntity>
    where TEntity : class
    {
        /// <summary>
        /// Create a list of new entities
        /// </summary>
        /// <param name="entities">List of entities</param>
        void CreateMany(IEnumerable<TEntity> entities);
    }
}

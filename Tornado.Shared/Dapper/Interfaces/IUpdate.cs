using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.Dapper.Interfaces
{
    public interface IUpdate<TEntity>
            where TEntity : class
    {
        /// <summary>
        /// Update an existing entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(TEntity entity);
    }
}

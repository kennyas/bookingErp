using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.Dapper.Interfaces
{
    public interface ICreate<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Create a new entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Create(TEntity entity);
    }
}

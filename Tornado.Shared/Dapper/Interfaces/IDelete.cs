﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.Dapper.Interfaces
{
    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IDelete<TEntity>
      where TEntity : class
    {
        /// <summary>
        /// Delete an existing entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);
    }
}

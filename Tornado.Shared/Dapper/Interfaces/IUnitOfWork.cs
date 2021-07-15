using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.Dapper.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Dispose(bool disposing);
        IDapperRepository<TEntity> Repository<TEntity>() where TEntity : class;
        void Commit();
        void Rollback();
    }
}

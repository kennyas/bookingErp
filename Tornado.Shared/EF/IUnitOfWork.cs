using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tornado.Shared.EF.Repository;
using Tornado.Shared.Models;

namespace Tornado.Shared.EF
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        void Dispose(bool disposing);
        IRepository<TEntity> Repository<TEntity>() where TEntity : AuditedEntity;
        void BeginTransaction();
        int Commit();
        void Rollback();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> CommitAsync();
    }
}

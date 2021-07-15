using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tornado.Shared.Dapper.Interfaces;

namespace Tornado.Shared.Dapper.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbTransaction _dbTransaction;
        private readonly IDbConnection _connection;

        public UnitOfWork(IDbConnection connectiom)
        {
            _connection = connectiom;
        }

        public IDapperRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return new DapperRepository<TEntity>(_connection);
        }

        public void BeginTransaction()
        {
            _dbTransaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            if (_dbTransaction == null)
                return;

            _dbTransaction.Commit();
        }

        public void Rollback()
        {
            if (_dbTransaction == null)
                return;

            _dbTransaction.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public virtual void Dispose(bool disposing)
        {
            GC.SuppressFinalize(this);
        }
    }

}

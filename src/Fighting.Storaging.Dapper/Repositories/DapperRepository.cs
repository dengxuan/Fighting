using Dapper.Contrib.Extensions;
using Fighting.Storaging.Abstractions;
using Fighting.Storaging.Entities.Abstractions;
using Fighting.Storaging.Repositories.Abstractions;
using System;
using System.Data;
using System.Linq;

namespace Fighting.Storaging.Dapper.Repositories
{

    public class DapperRepository<TEntity> : DapperRepository<TEntity, Guid> where TEntity : class, IEntity<Guid>
    {
        public DapperRepository(IActiveTransactionProvider dbConnection) : base(dbConnection) { }
    }

    public class DapperRepository<TEntity, TPrimaryKey> : Repository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly IActiveTransactionProvider _activeTransactionProvider;

        public DapperRepository(IActiveTransactionProvider activeTransactionProvider)
        {
            _activeTransactionProvider = activeTransactionProvider;
        }

        public override void Delete(TEntity entity)
        {
            using (IDbConnection db = _activeTransactionProvider.GetActiveConnection(ActiveTransactionProviderArgs.Empty))
            {
                db.Delete(entity);
            }
        }

        public override void Delete(TPrimaryKey id)
        {
            using (IDbConnection db = _activeTransactionProvider.GetActiveConnection(ActiveTransactionProviderArgs.Empty))
            {
                TEntity entity = db.Get<TEntity>(id);
                db.Delete(entity);
            }
        }

        public override IQueryable<TEntity> GetAll()
        {
            using (IDbConnection db = _activeTransactionProvider.GetActiveConnection(ActiveTransactionProviderArgs.Empty))
            {
                return db.GetAll<TEntity>().AsQueryable();
            }
        }

        public override TEntity Insert(TEntity entity)
        {
            using (IDbConnection db = _activeTransactionProvider.GetActiveConnection(ActiveTransactionProviderArgs.Empty))
            {
                db.Insert(entity);
                return entity;
            }
        }

        public override TEntity Update(TEntity entity)
        {
            using (IDbConnection db = _activeTransactionProvider.GetActiveConnection(ActiveTransactionProviderArgs.Empty))
            {
                db.Update(entity);
                return entity;
            }
        }
    }
}

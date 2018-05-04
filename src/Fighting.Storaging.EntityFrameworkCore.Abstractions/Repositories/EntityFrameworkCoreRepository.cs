using Fighting.Storaging.Entities.Abstractions;
using Fighting.Storaging.EntityFrameworkCore.Abstractions;
using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Fighting.Storaging.EntityFrameworkCore.Repositories
{

    public class EntityFrameworkCoreRepository<TDbContext, TEntity> : EntityFrameworkCoreRepository<TDbContext, TEntity, int>, IRepository<TEntity>, IRepository<TEntity, int> where TEntity : class, IEntity<int> where TDbContext : StorageContext
    {
        public EntityFrameworkCoreRepository(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

    }

    public class EntityFrameworkCoreRepository<TDbContext, TEntity, TPrimaryKey> : Repository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey> where TDbContext : StorageContext
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        /// <summary>
        /// Gets EF DbContext object.
        /// </summary>
        public virtual TDbContext Context => _dbContextProvider.GetDbContext();


        public EntityFrameworkCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public override void Delete(TEntity entity)
        {
            Context.Remove(entity);
            Context.SaveChanges();
        }

        public override void Delete(TPrimaryKey id)
        {
            TEntity entity = Context.Find<TEntity>(id);
            Delete(entity);
        }

        public override IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        public override TEntity Insert(TEntity entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
            return entity;
        }

        public override TEntity Update(TEntity entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
            return entity;
        }
    }
}

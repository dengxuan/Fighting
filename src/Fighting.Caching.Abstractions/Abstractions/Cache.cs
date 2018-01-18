using Fighting.Threading.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fighting.Caching.Abstractions
{
    public abstract class Cache : ICache
    {
        private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

        public string Name { get; }

        public TimeSpan DefaultSlidingExpireTime { get; set; }

        public TimeSpan? DefaultAbsoluteExpireTime { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        protected Cache(string name)
        {
            Name = name;
            DefaultSlidingExpireTime = TimeSpan.FromHours(1);
        }

        private object Retrieve(string key, Func<string, object> factory)
        {
            var result = factory(key);
            if (result != null)
            {
                Set(key, result);
            }
            return result;
        }

        private TEntity Retrieve<TEntity>(string key, Func<string, TEntity> factory)
        {
            var result = factory(key);
            if (result != null)
            {
                Set(key, result);
            }
            return result;
        }

        public abstract object GetOrDefault(string key);

        public abstract TEntity GetOrDefault<TEntity>(string key);

        public abstract Task<object> GetOrDefaultAsync(string key);

        public abstract Task<TEntity> GetOrDefaultAsync<TEntity>(string key);

        public virtual object Get(string key, Func<string, object> factory)
        {
            return locker.Locking((cacheKey) => GetOrDefault(cacheKey), (cacheKey) => Retrieve(cacheKey, factory), key);
        }

        public TEntity Get<TEntity>(string key, Func<string, TEntity> factory)
        {
            return locker.Locking((cacheKey) => GetOrDefault<TEntity>(cacheKey), (cacheKey) => Retrieve(cacheKey, factory), key);
        }

        public virtual async Task<object> GetAsync(string key, Func<string, object> factory)
        {
            return await locker.LockingAsync((cacheKey) => GetOrDefault(cacheKey), (cacheKey) => Retrieve(cacheKey, factory), key);
        }

        public virtual async Task<TEntity> GetAsync<TEntity>(string key, Func<string, TEntity> factory)
        {
            return await locker.LockingAsync((cacheKey) => GetOrDefault<TEntity>(cacheKey), (cacheKey) => Retrieve(cacheKey, factory), key);
        }

        public abstract void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        public abstract void Set<TEntity>(string key, TEntity value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        public abstract Task SetAsync(string key, Task<object> value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        public abstract Task SetAsync<TEntity>(string key, Task<TEntity> value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        public abstract void Remove(string key);

        public virtual async Task RemoveAsync(string key)
        {
            await Task.Run(() => Remove(key));
        }

        public abstract void Clear();

        public virtual async Task ClearAsync()
        {
            await Task.Run(() => Clear());
        }

        public virtual void Dispose()
        {

        }
    }
}

using Fighting.Caching.Abstractions;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Fighting.Caching.Redis
{
    public class RedisCache : Cache
    {
        private readonly IDatabase _database;
        private readonly ICachingSerializer _serializer;

        internal RedisCache(IRedisCacheProvider redisCacheProvider, ICachingSerializer serializer, string name) : base(name)
        {
            _database = redisCacheProvider.GetDatabase();
            _serializer = serializer;
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override object GetOrDefault(string key)
        {
            var objbyte = _database.StringGet(key);
            return objbyte.HasValue ? _serializer.Deserialize<object>(objbyte) : null;
        }

        public override TEntity GetOrDefault<TEntity>(string key)
        {
            var objbyte = _database.StringGet(key);
            return objbyte.HasValue ? _serializer.Deserialize<TEntity>(objbyte) : default(TEntity);
        }

        public async override Task<object> GetOrDefaultAsync(string key)
        {
            var objbyte = await _database.StringGetAsync(key);
            return objbyte.HasValue ? _serializer.Deserialize<object>(objbyte) : null;
        }

        public async override Task<TEntity> GetOrDefaultAsync<TEntity>(string key)
        {
            var objbyte = await _database.StringGetAsync(key);
            return objbyte.HasValue ? _serializer.Deserialize<TEntity>(objbyte) : default(TEntity);
        }

        public override void Remove(string key)
        {
            _database.KeyDelete(key);
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new Exception("Can not insert null values to the cache!");
            }
            byte[] bytes = _serializer.Serialize(value);
            _database.StringSet(key, bytes, absoluteExpireTime ?? slidingExpireTime ?? DefaultAbsoluteExpireTime ?? DefaultSlidingExpireTime);
        }

        public override void Set<TEntity>(string key, TEntity value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new Exception("Can not insert null values to the cache!");
            }
            byte[] bytes = _serializer.Serialize(value);
            _database.StringSet(key, bytes, absoluteExpireTime ?? slidingExpireTime ?? DefaultAbsoluteExpireTime ?? DefaultSlidingExpireTime);
        }

        public override async Task SetAsync(string key, Task<object> value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            object @object = await value;
            if (value == null)
            {
                throw new Exception("Can not insert null values to the cache!");
            }
            byte[] bytes = _serializer.Serialize(@object);
            await _database.StringSetAsync(key, bytes, absoluteExpireTime ?? slidingExpireTime ?? DefaultAbsoluteExpireTime ?? DefaultSlidingExpireTime);
        }

        public override async Task SetAsync<TEntity>(string key, Task<TEntity> value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            TEntity entity = await value;
            if (value == null)
            {
                throw new Exception("Can not insert null values to the cache!");
            }
            byte[] bytes = _serializer.Serialize(entity);
            await _database.StringSetAsync(key, bytes, absoluteExpireTime ?? slidingExpireTime ?? DefaultAbsoluteExpireTime ?? DefaultSlidingExpireTime);
        }
    }
}

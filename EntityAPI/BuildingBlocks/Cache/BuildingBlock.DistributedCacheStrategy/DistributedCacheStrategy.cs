using BuildingBlock.Cache.Abstraction;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace BuildingBlock.DistributedCacheStrategy
{
    public class DistributedCacheStrategy : IDistributedCacheStrategy
    {
        private readonly IDistributedCache _cache;

        public DistributedCacheStrategy(IDistributedCache cache)
        {
            this._cache = cache;
        }

        public async Task<List<T>> GetListAsync<T>(string key, Func<Task<IEnumerable<T>>> getCallFunc,
            int slidingExpiration = 5,
            int absoluteExpiration = 2)
        {
            var cachedEntities = await this._cache.GetAsync(key);

            var entitiesList = new List<T>();

            string serializedEntities = string.Empty;

            if (cachedEntities != null)
            {
                serializedEntities = Encoding.UTF8.GetString(cachedEntities);

                entitiesList = JsonConvert.DeserializeObject<List<T>>(serializedEntities);
            }
            else
            {
                var entitiesListCall = await getCallFunc();

                entitiesList = entitiesListCall.ToList();

                serializedEntities = JsonConvert.SerializeObject(entitiesList);
                cachedEntities = Encoding.UTF8.GetBytes(serializedEntities);

                var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiration))
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(absoluteExpiration));

                await _cache.SetAsync(key, cachedEntities, options);
            }

            return entitiesList;
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> getCallFunc,
            int slidingExpiration = 5,
            int absoluteExpiration = 2) where T : new()
        {
            var cachedEntity = await this._cache.GetAsync(key);

            var entity = new T();

            string serializedEntity = string.Empty;

            if (cachedEntity != null)
            {
                serializedEntity = Encoding.UTF8.GetString(cachedEntity);

                entity = JsonConvert.DeserializeObject<T>(serializedEntity);
            }
            else
            {
                entity = await getCallFunc();

                serializedEntity = JsonConvert.SerializeObject(entity);
                cachedEntity = Encoding.UTF8.GetBytes(serializedEntity);

                var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiration))
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(absoluteExpiration));

                await _cache.SetAsync(key, cachedEntity, options);
            }

            return entity;
        }

        public async Task UpdateAsync<T>(string key, T entity,
            int slidingExpiration = 5,
            int absoluteExpiration = 2)
        {
            var serializedEntity = JsonConvert.SerializeObject(entity);
            var cachedEntity = Encoding.UTF8.GetBytes(serializedEntity);

            var options = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiration))
                        .SetAbsoluteExpiration(DateTime.Now.AddHours(absoluteExpiration));

            await Remove(key);

            await _cache.SetAsync(key, cachedEntity, options);
        }

        public async Task Remove(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
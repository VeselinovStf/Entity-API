namespace BuildingBlock.Cache.Abstraction
{
    public interface IDistributedCacheStrategy
    {
        Task<T> GetAsync<T>(string key, Func<Task<T>> getCallFunc, int slidingExpiration = 5, int absoluteExpiration = 2) where T : new();
        Task<List<T>> GetListAsync<T>(string key, Func<Task<IEnumerable<T>>> getCallFunc, int slidingExpiration = 5, int absoluteExpiration = 2);
        Task UpdateAsync<T>(string key, T entity,
            int slidingExpiration = 5,
            int absoluteExpiration = 2);

        Task Remove(string key);
    }
}
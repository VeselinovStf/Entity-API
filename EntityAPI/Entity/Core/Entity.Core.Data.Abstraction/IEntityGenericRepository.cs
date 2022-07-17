namespace Entity.Core.Data.Abstraction
{
    public interface IEntityGenericRepository<T> : IDisposable
        where T : IBaseEntity, new()
    {
        IQueryable<T> GetAll();
        Task<T> AddAsync(T entity);
        Task SaveChangesAsync();
        Task<T> Get(int id);
    }
}

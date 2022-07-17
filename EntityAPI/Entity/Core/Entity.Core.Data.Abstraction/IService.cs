namespace Entity.Core.Data.Abstraction
{
    public interface IService<T>
    {
        Task<IList<T>> GetAllAsync();
    }
}
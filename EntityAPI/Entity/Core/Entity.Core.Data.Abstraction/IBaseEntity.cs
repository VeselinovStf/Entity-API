namespace Entity.Core.Data.Abstraction
{
    public interface IBaseEntity : ICreatable, IModifiable, IDeletable
    {
        int Id { get; set; }
    }
}
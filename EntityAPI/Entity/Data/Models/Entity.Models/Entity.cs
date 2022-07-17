using Entity.Core.Data.Abstraction;

namespace Entity.Models
{
    public class Entity : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
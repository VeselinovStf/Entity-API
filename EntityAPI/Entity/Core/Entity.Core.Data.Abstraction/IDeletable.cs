using System;

namespace Entity.Core.Data.Abstraction
{
    public interface IDeletable
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}

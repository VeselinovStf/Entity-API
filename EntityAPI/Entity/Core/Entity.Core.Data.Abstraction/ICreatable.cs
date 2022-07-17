using System;

namespace Entity.Core.Data.Abstraction
{
    public interface ICreatable
    {
        DateTime? CreatedAt { get; set; }
    }
}

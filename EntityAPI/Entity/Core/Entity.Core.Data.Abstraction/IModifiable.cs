using System;

namespace Entity.Core.Data.Abstraction
{
    public interface IModifiable
    {
        DateTime? ModifiedAt { get; set; }
    }
}

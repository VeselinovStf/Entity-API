using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Data.Provider.SQLServer.Configuration
{
    public class EntityModelConfiguration : IEntityTypeConfiguration<Models.Entity>
    {
        public void Configure(EntityTypeBuilder<Models.Entity> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}

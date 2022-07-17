using Entity.Core.Data.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Entity.Data.Provider.SQLServer
{
    public class EntitySQLServerDbContext : DbContext
    {
        public DbSet<Models.Entity> Entities { get; set; }
        public EntitySQLServerDbContext(DbContextOptions<EntitySQLServerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntitySQLServerDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            this.ApplyAuditInfoRules();
            return await base.SaveChangesAsync();
        }

        private void ApplyAuditInfoRules()
        {
            var newlyCreatedEntities = this.ChangeTracker.Entries()
                .Where(e => e.Entity is IModifiable && ((e.State == EntityState.Added) || (e.State == EntityState.Modified)));

            foreach (var entry in newlyCreatedEntities)
            {
                var entity = (IBaseEntity)entry.Entity;

                if (entry.State == EntityState.Added && entity.CreatedAt == null)
                {
                    entity.CreatedAt = DateTime.Now;
                }
                else
                {
                    entity.ModifiedAt = DateTime.Now;

                    if (entity.IsDeleted == true)
                    {
                        entity.DeletedOn = DateTime.Now;
                    }
                }
            }
        }
    }
}
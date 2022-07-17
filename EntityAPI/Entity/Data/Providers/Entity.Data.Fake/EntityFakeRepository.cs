using Entity.Core.Data.Abstraction;

namespace Entity.Data.Fake
{
    public class EntityFakeRepository : IEntityGenericRepository<Entity.Models.Entity>
    {
        private readonly EntityFakeDbContext context;

        public EntityFakeRepository(EntityFakeDbContext context)
        {
            this.context = context;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // context.Dispose
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IQueryable<Entity.Models.Entity> GetAll()
        {
            return this.context._entities.AsQueryable();
        }

        public Task<Models.Entity> AddAsync(Models.Entity entity)
        {
            var id = context._entities.OrderByDescending(x => x.Id).First().Id + 1;

            entity.Id = id;

            context._entities.Add(entity);

            return Task.FromResult(entity);
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }

        public Task<Models.Entity> Get(int id)
        {
            return Task.FromResult(context._entities.FirstOrDefault(e => e.Id == id && !e.IsDeleted));
        }
    }
}

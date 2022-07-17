using Entity.Core.Data.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Entity.Data.Provider.SQLServer
{
    public class EntitySQLServerRepository : IEntityGenericRepository<Entity.Models.Entity>
    {
        private readonly EntitySQLServerDbContext _context;

        public EntitySQLServerRepository(EntitySQLServerDbContext context)
        {
            this._context = context;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this._context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IQueryable<Models.Entity> GetAll()
        {
            return this._context.Entities
                .Where(e => !e.IsDeleted)
                .AsQueryable();
        }

        public async Task<Models.Entity> AddAsync(Models.Entity entity)
        {
            var entityAddResult = await this._context.AddAsync(entity);

            return entityAddResult.Entity;
        }

        public async Task SaveChangesAsync()
        {
            await this._context.SaveChangesAsync();
        }

        public async Task<Models.Entity> Get(int id)
        {
            return await this._context.Entities.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }
    }
}

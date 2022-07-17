namespace Entity.Data.Fake
{
    public class EntityFakeDbContext
    {
        public List<Models.Entity> _entities { get; set; }

        public EntityFakeDbContext()
        {
            _entities = new List<Models.Entity>()
            {
                new Models.Entity(){Id=1},
                new Models.Entity(){Id=2},
                new Models.Entity(){Id=3},
                new Models.Entity(){Id=4},
                new Models.Entity(){Id=5},
                new Models.Entity(){Id=6},
            };
        }

        public async Task Add(Models.Entity product)
        {
            _entities.Add(product);
            await Task.CompletedTask;
        }
        public async Task<IEnumerable<Models.Entity>> Get() => await Task.FromResult(_entities);
    }
}
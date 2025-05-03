using Microsoft.EntityFrameworkCore;

using Rommanel.Infra.Entity;

namespace Rommanel.Infra.Context
{
    public partial class ClientContext : DbContext
    {

        public ClientContext(DbContextOptions<ClientContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> clientes { get; set; }
        public DbSet<Logradouro> logradouros { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
            .UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

    }
}

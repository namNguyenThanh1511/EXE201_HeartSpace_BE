using HeartSpace.Domain.Entities;
using HeartSpace.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;


namespace HeartSpace.Infrastructure.Persistence
{
    public class RepositoryContext : DbContext
    {

        public RepositoryContext(DbContextOptions<RepositoryContext> options)
            : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your entities here
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
        public override int SaveChanges()
        {
            ConvertLocalDateTimesToUtc();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ConvertLocalDateTimesToUtc();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ConvertLocalDateTimesToUtc()
        {
            foreach (var entry in ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                foreach (var prop in entry.Properties
                    .Where(p => p.Metadata.ClrType == typeof(DateTime)))
                {
                    var dt = (DateTime)prop.CurrentValue;
                    if (dt.Kind == DateTimeKind.Local)
                        prop.CurrentValue = dt.ToUniversalTime();
                }
            }
        }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}

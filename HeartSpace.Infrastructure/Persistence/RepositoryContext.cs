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
            modelBuilder.ApplyConfiguration(new ClientProfileConfiguration());
            modelBuilder.ApplyConfiguration(new ConsultantProfileConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new AppointmentConfiguration());

        }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }



    }
}

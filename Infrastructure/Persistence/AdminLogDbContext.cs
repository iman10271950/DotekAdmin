using Application.Common.InterFaces.DbContext;
using Domain.Entities.Log;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AdminLogDbContext : DbContext, IAdminLogDbContext
    {
        public AdminLogDbContext(DbContextOptions<AdminLogDbContext> options) : base(options)
        {

        }
        public DbSet<RequestAdminLog> RequestAdminLog { get; set; }

        public DbSet<ResponseAdminLog> ResponseAdminLog { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                try
                {
                }
                catch (Exception)
                {
                    throw new Exception("Database Config has a problem");
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

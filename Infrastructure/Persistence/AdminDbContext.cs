
using Application.Common.InterFaces.DbContext;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Common;
using Domain.Entities.AdminSetting;
using Domain.Entities.Auth;
using Domain.Entities.ServiceLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;
using System.Reflection;

namespace Infrastructure.Persistence;

public class AdminDbContext : DbContext, IAdminDbContext
{
    public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
    {
        this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
    }

    #region Auth Entities

    public DbSet<User> User { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Access> Access { get; set; }
    public DbSet<UserAccess> UserAccess { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<RoleAccess> RoleAccess { get; set; }
    public DbSet<Session> Session { get; set; }
    public DbSet<Activation> Activation { get; set; }

    #endregion

    #region ServiceLog

    public DbSet<ServiceLog> ServiceLogs { get; set; }
    public DbSet<ServiceLogEntity> ServiceLogEntity { get; set; }
    #endregion

    #region Admin Settings
    public DbSet<AdminSetting> AdminSetting { get; set; }

    #endregion

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        int count = await base.SaveChangesAsync(cancellationToken);
        this.ChangeTracker.Clear();
        return count;
    }
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await Database.BeginTransactionAsync(cancellationToken);
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        //if (!optionsBuilder.IsConfigured)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception)
        //    {
        //        // throw new Exception("Database Config has a problem");
        //    }
        //}
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.UseCollation("Persian_100_CI_AS");

        modelBuilder.Entity<User>()
            .HasMany(u => u.Accesses)
            .WithMany(a => a.Users)
            .UsingEntity<UserAccess>(
                ua => ua.HasOne(ua => ua.Access).WithMany().HasForeignKey(ua => ua.AccessId),
                ua => ua.HasOne(ua => ua.User).WithMany().HasForeignKey(ua => ua.UserId),
                ua =>
                {
                    ua.ToTable("UserAccess");
                    ua.Property(ua => ua.IsAdded).HasColumnName("IsAdded");
                    ua.HasKey(ua => new { ua.UserId, ua.AccessId });
                }
            );

        modelBuilder.Entity<User>()
           .HasMany(u => u.Roles)
           .WithMany(a => a.Users)
           .UsingEntity<UserRole>(
             ua => ua.HasOne(ua => ua.Role).WithMany().HasForeignKey(ua => ua.RoleId),
                ua => ua.HasOne(ua => ua.User).WithMany().HasForeignKey(ua => ua.UserId),
                ua =>
                {
                    ua.ToTable("UserRole");
                    ua.HasKey(ua => new { ua.UserId, ua.RoleId });
                });

        modelBuilder.Entity<Role>()
          .HasMany(u => u.Accesses)
          .WithMany(a => a.Roles)
          .UsingEntity<RoleAccess>(
             ua => ua.HasOne(ua => ua.Access).WithMany().HasForeignKey(ua => ua.AccessId),
                ua => ua.HasOne(ua => ua.Role).WithMany().HasForeignKey(ua => ua.RoleId),
                ua =>
                {
                    ua.ToTable("RoleAccess");
                    ua.HasKey(ua => new { ua.RoleId, ua.AccessId });
                });

        modelBuilder.Entity<Session>()
            .HasIndex(m => m.Token)
            .IsUnique();


        modelBuilder.Entity<ServiceLogEntity>()
            .HasMany(m => m.ServiceLog)
            .WithOne(n => n.ServiceLogEntity)
            .HasForeignKey(i => i.EntityId);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());



        base.OnModelCreating(modelBuilder);
    }

    
}

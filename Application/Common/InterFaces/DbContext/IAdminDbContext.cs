using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.ServiceLog;
using Domain.Entities.AdminSetting;

namespace Application.Common.InterFaces.DbContext
{
    public interface IAdminDbContext
    {
        public DbSet<User> User { get; }
        public DbSet<Role> Role { get; }
        public DbSet<Access> Access { get; }
        public DbSet<UserAccess> UserAccess { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<RoleAccess> RoleAccess { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<Activation> Activation { get; set; }


        public DbSet<AdminSetting> AdminSetting { get; set; }


        public DbSet<ServiceLog> ServiceLogs { get; set; }
        public DbSet<ServiceLogEntity> ServiceLogEntity { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}

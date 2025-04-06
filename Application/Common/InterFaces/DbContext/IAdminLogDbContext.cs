using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Log;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.InterFaces.DbContext
{
    public interface IAdminLogDbContext
    {
        public DbSet<RequestAdminLog> RequestAdminLog { get; }
        public DbSet<ResponseAdminLog> ResponseAdminLog { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

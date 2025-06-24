
using Application.Common.InterFaces.DbContext;
using Domain.Entities.Log;
using Microsoft.Extensions.Logging;

namespace Application.Common.Methodes
{
    public class LogerMethode :Application.Common.Interfaces.Logger.ILogger
    {
        private readonly IAdminLogDbContext _context;

        public LogerMethode(IAdminLogDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Log(RequestAdminLog request)
        {

            _context.RequestAdminLog.Add(request);
            await _context.SaveChangesAsync(CancellationToken.None);
            return true;
        }

        public async Task<bool> Log(ResponseAdminLog response)
        {

            _context.ResponseAdminLog.Add(response);
            await _context.SaveChangesAsync(CancellationToken.None);
            return true;
        }
    }
}
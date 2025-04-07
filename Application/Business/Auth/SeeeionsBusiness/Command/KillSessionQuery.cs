
using Application.Common.BaseEntities;
using Application.Common.InterFaces.DbContext;
using Domain.Entities.Auth;
using Domain.Enums;
using Domain.Enums.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.Auth.Session.Query
{
    public class KillSessionQuery : IRequest<BaseResult_VM<string>>
    {
        public Guid Id { get; set; }
    }

    public class KillSessionHandler : IRequestHandler<KillSessionQuery, BaseResult_VM<string>>
    {
        private readonly IAdminDbContext _dbContext;

        public KillSessionHandler(IAdminDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResult_VM<string>> Handle(KillSessionQuery query, CancellationToken cancellationToken)
        {
            if (query.Id == null) return new BaseResult_VM<string> { Code = -1, Message = "نشست مورد نظر یافت نشد" };

            var session = await GetSession(query.Id);
            if (session == null) return new BaseResult_VM<string> { Code = -1, Message = "نشست مورد نظر یافت نشد" };

            session.Status = (int)SessionStatus.Inactive;
            _dbContext.Session.Update(session);

            if (await _dbContext.SaveChangesAsync(cancellationToken) <= 0) return new BaseResult_VM<string> { Code = -1, Message = "نشست مورد نظر یافت نشد" };

            return new BaseResult_VM<string>("", 0, "نشست با موفقیت غیر فعال شد");
        }

        public async Task<Domain.Entities.Auth.Session?> GetSession(Guid id)
        {
            var session = await _dbContext.Session.Where(m => m.Id == id).FirstOrDefaultAsync();
            return session;
        }
    }
}

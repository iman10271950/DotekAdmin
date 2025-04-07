using Application.Common.BaseEntities;
using Application.Common.InterFaces.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.Auth.Activation.Query
{
    public class GetActivationListQuery : IRequest<BaseResult_VM<List<Domain.Entities.Auth.Activation>>>
    {
        public string PhoneNumber { get; set; }
        public int? ActivationCode { get; set; }
        public int Status { get; set; }
    }

    public class GetActivationListHandler : IRequestHandler<GetActivationListQuery, BaseResult_VM<List<Domain.Entities.Auth.Activation>>>
    {
        private readonly IAdminDbContext _dbcontext;

        public GetActivationListHandler(IAdminDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public async Task<BaseResult_VM<List<Domain.Entities.Auth.Activation>>> Handle(GetActivationListQuery request, CancellationToken cancellationToken)
        {
            var activationList = from act in _dbcontext.Activation.AsQueryable()
                                 join usr in _dbcontext.User.AsQueryable() on act.UserId equals usr.Id
                                 select new { User = usr, Activation = act };

            if (request.PhoneNumber != null)
                activationList = activationList.Where(m => m.User.MobileNumber == request.PhoneNumber);

            if (request.ActivationCode != null)
                activationList = activationList.Where(m => m.Activation.ActivationCode == request.ActivationCode);

            if (request.Status != null)
                activationList = activationList.Where(m => m.Activation.Status == request.Status);

            var list = await activationList.Select(m => m.Activation).ToListAsync();

            return new BaseResult_VM<List<Domain.Entities.Auth.Activation>>(list, 0, "");
        }
    }
}
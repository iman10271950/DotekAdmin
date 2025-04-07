
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
    public class GetActivationQuery : IRequest<BaseResult_VM<Domain.Entities.Auth.Activation?>>
    {
        public string PhoneNumber { get; set; }
        public int ActivationCode { get; set; }
        public int Status { get; set; }
    }

    public class GetActivationHandler : IRequestHandler<GetActivationQuery, BaseResult_VM<Domain.Entities.Auth.Activation?>>
    {
        private readonly IAdminDbContext _dbcontext;
        public GetActivationHandler(IAdminDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public async Task<BaseResult_VM<Domain.Entities.Auth.Activation?>> Handle(GetActivationQuery request, CancellationToken cancellationToken)
        {
            var activation = await (from act in _dbcontext.Activation.AsQueryable()
                                    join usr in _dbcontext.User.AsQueryable() on act.UserId equals usr.Id
                                    where usr.MobileNumber == request.PhoneNumber &&
                                    act.Status == request.Status &&
                                    act.ActivationCode == request.ActivationCode
                                    select act).FirstOrDefaultAsync();

            return new BaseResult_VM<Domain.Entities.Auth.Activation>(activation, 0, "");
        }
    }
}

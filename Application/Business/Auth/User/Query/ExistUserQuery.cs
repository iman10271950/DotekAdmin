
using Application.Common.BaseEntities;
using Application.Common.InterFaces.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.Auth.User.Query
{
    public class ExistUserQuery:IRequest<BaseResult_VM<bool>>
    {
        public string PhoneNumber { get; set; }
    }
    public class ExistUserQueryHandler : IRequestHandler<ExistUserQuery, BaseResult_VM<bool>>
    {
        private readonly IAdminDbContext _dbContext;

        public ExistUserQueryHandler(IAdminDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<BaseResult_VM<bool>> Handle(ExistUserQuery request, CancellationToken cancellationToken)
        {
            return new BaseResult_VM<bool>
            {
                Code = 0,
                Result =  _dbContext.User.Any(x => x.MobileNumber == request.PhoneNumber)
            };
        }
    }
}

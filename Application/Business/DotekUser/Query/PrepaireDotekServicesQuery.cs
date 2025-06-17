using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Business.DotekUser.VieModel;
using Application.Common.BaseEntities;
using Application.Common.Extentions;
using Domain.Entities.Auth;
using Domain.Enums;
using Domain.Enums.Auth;
using MediatR;

namespace Application.Business.DotekUser.Query
{
    public class PrepaireDotekServicesQuery:IRequest<BaseResult_VM<PrepairDotekService_VM>>
    {
    }
    public class PrepaireDotekServicesQueryHandler : IRequestHandler<PrepaireDotekServicesQuery, BaseResult_VM<PrepairDotekService_VM>>
    {
        
        public async Task<BaseResult_VM<PrepairDotekService_VM>> Handle(PrepaireDotekServicesQuery request, CancellationToken cancellationToken)
        {
           var res =new PrepairDotekService_VM();
            res.UserStatus=Domain.Enums.Auth.UserStatus.Active.ToBaseEnumList();
            res.RequestStatusList=OrderStatus.Active.ToBaseEnumList();
            return new BaseResult_VM<PrepairDotekService_VM>
            {
                Code = 0,
                Message = "با موفقیت انجام شد",
                Result = res
            };
        }
    }
}

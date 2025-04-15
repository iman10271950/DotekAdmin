using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Business.DotekRequest.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.InterFaces.Services;
using Application.Common.Messager.Enums;
using Application.Common.Models;
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.DotekRequest.Query
{
  
    public class GetlAllDotekRequestQuery : IRequest<BaseResult_VM<PaginatedList<Request_VM>>>
    {
        public RequestListSearch_VM SearchItems { get; set; }
        public OtherServicesAuth_VM Auth { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class GetlAllRequestForAdminQueryHandler : IRequestHandler<GetlAllDotekRequestQuery, BaseResult_VM<PaginatedList<Request_VM>>>
    {
        private readonly IClientMessager _clientMessager;
        private readonly IAuthHelper _auth;

        public GetlAllRequestForAdminQueryHandler(IClientMessager clientMessager,IAuthHelper auth)
        {
           _clientMessager = clientMessager;
           _auth = auth;
        }
        public async Task<BaseResult_VM<PaginatedList<Request_VM>>> Handle(GetlAllDotekRequestQuery request, CancellationToken cancellationToken)
        {
            var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<PaginatedList<Request_VM>>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Request_GetlAllRequestForAdmin");
            if (methodResult.Code == 103)
            {
                return methodResult.Result;
            }
            if (!methodResult.Success)
            {
                return new BaseResult_VM<PaginatedList<Request_VM>>(null, methodResult.Code, "خطا در فراخوانی سرویس دریافت لیست درخواست ها", false);
            }

            return methodResult.Result;
        }
    }
}

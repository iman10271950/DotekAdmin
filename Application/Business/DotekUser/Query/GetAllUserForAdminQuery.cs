using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Business.DotekRequest.ViewModel;
using Application.Business.DotekUser.VieModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.InterFaces.Services;
using Application.Common.Messager.Enums;
using Application.Common.Models;
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.DotekUser.Query
{
    public class GetAllUserForAdminQuery:  IRequest<BaseResult_VM<PaginatedList<DotekUser_VM>>>
    {
        public OtherServicesAuth_VM Auth { get; set; }
        public DotekUser_VM SearchItems { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class GetAllUserForAdminQueryHandler : IRequestHandler<GetAllUserForAdminQuery, BaseResult_VM<PaginatedList<DotekUser_VM>>>
    {
        private readonly IClientMessager _clientMessager;
        private readonly IAuthHelper _auth;

        public GetAllUserForAdminQueryHandler(IClientMessager clientMessager , IAuthHelper auth)
        {
            _clientMessager = clientMessager;
            _auth = auth;
        }
        public async Task<BaseResult_VM<PaginatedList<DotekUser_VM>>> Handle(GetAllUserForAdminQuery request, CancellationToken cancellationToken)
        {
           request.Auth = _auth.GetDefaultAuth();
           
            
            
            var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<PaginatedList<DotekUser_VM>>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_GetAllUserForAdmin");
            if (methodResult.Code == 103)
            {
                return methodResult.Result;
            }
            if (!methodResult.Success)
            {
                return new BaseResult_VM<PaginatedList<DotekUser_VM>>(null, methodResult.Code, "خطا در فراخوانی سرویس دریافت لیست کاربران", false);
            }

            return methodResult.Result;
        }
    }
}

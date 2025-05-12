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

namespace Application.Business.DotekUser.Query
{
    public class GetAllRolesQuery : IRequest<BaseResult_VM<List<DotekRole_VM>>>
    {
        public OtherServicesAuth_VM Auth { get; set; }

    }
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, BaseResult_VM<List<DotekRole_VM>>>
    {
        private readonly IClientMessager _clientMessager;
        private readonly IAuthHelper _auth;

        public GetAllRolesQueryHandler(IClientMessager clientMessager, IAuthHelper auth)
        {
            _clientMessager = clientMessager;
            _auth = auth;
        }
        public async Task<BaseResult_VM<List<DotekRole_VM>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            request.Auth = _auth.GetDefaultAuth();
            var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<List<DotekRole_VM>>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_GetAllRoles");
            if (methodResult.Code == 103)
            {
                return methodResult.Result;
            }
            if (!methodResult.Success)
            {
                return new BaseResult_VM<List<DotekRole_VM>>(null, methodResult.Code, "خطا در فراخوانی سرویس دریافت لیست نقش ها", false);
            }

            return methodResult.Result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Business.DotekRequest.DotekUser.VieModel;
using Application.Business.DotekRequest.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.InterFaces.Services;
using Application.Common.Messager.Enums;
using Application.Common.Models;
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.DotekRequest.DotekUser.Query
{
    public class GetUserInformationWithIdInAdminQuery : IRequest<BaseResult_VM<DotekUser_VM>>
    {
        public OtherServicesAuth_VM Auth { get; set; }
        public long UserId { get; set; }
    }
    public class GetUserInformationWithIdInAdminQueryHandler : IRequestHandler<GetUserInformationWithIdInAdminQuery, BaseResult_VM<DotekUser_VM>>
    {
        private readonly IClientMessager _clientMessager;
        private readonly IAuthHelper _auth;

        public GetUserInformationWithIdInAdminQueryHandler(IClientMessager clientMessager,IAuthHelper auth)
        {
            _clientMessager = clientMessager;
            _auth = auth;
        }
        public async Task<BaseResult_VM<DotekUser_VM>> Handle(GetUserInformationWithIdInAdminQuery request, CancellationToken cancellationToken)
        {
            request.Auth = _auth.GetDefaultAuth();



            var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<DotekUser_VM>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_GetUserInformationWithIdInAdmin");
            if (methodResult.Code == 103)
            {
                return methodResult.Result;
            }
            if (!methodResult.Success)
            {
                return new BaseResult_VM<DotekUser_VM>(null, methodResult.Code, "خطا در فراخوانی سرویس دریافت اطلاعات کاربر", false);
            }

            return methodResult.Result;
        }
    }
}

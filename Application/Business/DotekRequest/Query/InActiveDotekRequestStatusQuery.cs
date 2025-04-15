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
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.DotekRequest.Query
{
    
    public class InActiveDotekRequestStatusQuery : IRequest<BaseResult_VM<bool>>
    {
        public OtherServicesAuth_VM Auth { get; set; }
        public long RequestId { get; set; }
    }
    public class InActiveRequestStatusCommandHandler : IRequestHandler<InActiveDotekRequestStatusQuery, BaseResult_VM<bool>>
    {
        private readonly IClientMessager _clientMessager;
        private readonly IAuthHelper _auth;

        public InActiveRequestStatusCommandHandler(IClientMessager clientMessager,IAuthHelper auth)
        {
            _clientMessager = clientMessager;
            _auth = auth;
        }
        public async Task<BaseResult_VM<bool>> Handle(InActiveDotekRequestStatusQuery request, CancellationToken cancellationToken)
        {
            request.Auth = _auth.GetDefaultAuth();
            var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<bool>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_InActiveRequestStatus");
            if (!methodResult.Success)
            {
                return new BaseResult_VM<bool>(false, methodResult.Code, "خطا در فراخوانی سرویس غیرفعالسازی درخواست");
            }

            return methodResult.Result;
        }
    }
}

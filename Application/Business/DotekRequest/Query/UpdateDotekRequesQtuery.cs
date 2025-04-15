using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Business.DotekRequest.ViewModel;
using Application.Business.Product.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.InterFaces.Services;
using Application.Common.Messager.Enums;
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.DotekRequest.Query
{

    public class UpdateDotekRequesQtuery : IRequest<BaseResult_VM<bool>>
    {
        public OtherServicesAuth_VM Auth { get; set; }
        public Request_VM Input { get; set; }
        public List<OtherProperty_VM> OtherProperty { get; set; }
    }
    public class UpdateRequestCommandHandler : IRequestHandler<UpdateDotekRequesQtuery, BaseResult_VM<bool>>
    {
        private readonly IClientMessager _clientMessager;
        private readonly IAuthHelper _auth;

        public UpdateRequestCommandHandler(IClientMessager clientMessager, IAuthHelper auth)
        {
            _clientMessager = clientMessager;
            _auth = auth;
        }
        public async Task<BaseResult_VM<bool>> Handle(UpdateDotekRequesQtuery request, CancellationToken cancellationToken)
        {
            request.Auth = _auth.GetDefaultAuth();
            var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<bool>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_UpdateRequest");
            if (!methodResult.Success)
            {
                return new BaseResult_VM<bool>(false, methodResult.Code, "خطا در فراخوانی سرویس به روز رسانی درخواست");
            }

            return methodResult.Result;
        }
    }
}

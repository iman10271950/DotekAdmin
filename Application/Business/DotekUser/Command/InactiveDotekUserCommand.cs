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

namespace Application.Business.DotekUser.Command
{
    public class InactiveDotekUserCommand:IRequest<BaseResult_VM<bool>>
    {
        public long UserId { get; set; }
        public OtherServicesAuth_VM Auth { get; set; }
    }
    public class InactiveDotekUserCommandHandler : IRequestHandler<InactiveDotekUserCommand, BaseResult_VM<bool>>
    {
        private readonly IClientMessager _clientMessager;
        private readonly IAuthHelper _auth;

        public InactiveDotekUserCommandHandler(IClientMessager clientMessager,IAuthHelper auth)
        {
           _clientMessager = clientMessager;
           _auth = auth;
        }
        public async Task<BaseResult_VM<bool>> Handle(InactiveDotekUserCommand request, CancellationToken cancellationToken)
        {
            request.Auth = _auth.GetDefaultAuth();
            var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<bool>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_InactivUser");
            if (!methodResult.Success)
            {
                return new BaseResult_VM<bool>(false, methodResult.Code, "خطا در فراخوانی سرویس غیرفعالسازی  کاربر");
            }

            return methodResult.Result;
        }
    }
}

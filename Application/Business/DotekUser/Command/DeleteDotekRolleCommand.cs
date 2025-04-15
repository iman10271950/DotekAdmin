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
    public class DeleteDotekRolleCommand : IRequest<BaseResult_VM<bool>>
    {
        public OtherServicesAuth_VM Auth { get; set; }
        public DotekRole_VM Role { get; set; }
    }
    public class DeleteDotekRolleCommandHandler : IRequestHandler<DeleteDotekRolleCommand, BaseResult_VM<bool>>
    {
        private readonly IClientMessager _clientMessager;
        private readonly IAuthHelper _auth;

        public DeleteDotekRolleCommandHandler(IClientMessager clientMessager,IAuthHelper auth)
        {
            _clientMessager = clientMessager;
            _auth = auth;
        }
        public async Task<BaseResult_VM<bool>> Handle(DeleteDotekRolleCommand request, CancellationToken cancellationToken)
        {
            request.Auth = _auth.GetDefaultAuth();
            var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<bool>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_DeleteRoleInAdmin");
            if (!methodResult.Success)
            {
                return new BaseResult_VM<bool>(false, methodResult.Code, "خطا در فراخوانی سرویس حذف نقش", false);
            }

            return methodResult.Result;
        }
    }
}

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

namespace Application.Business.DotekRequest.DotekUser.Command
{
    public class UpdateUserCommand : IRequest<BaseResult_VM<bool>>
    {
        public OtherServicesAuth_VM Auth { get; set; }
        public DotekUser_VM User { get; set; }
    }
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, BaseResult_VM<bool>>
    {
        private readonly IClientMessager _clientMessager;
        private readonly IAuthHelper _auth;

        public UpdateUserCommandHandler(IClientMessager clientMessager, IAuthHelper auth)
        {
            _clientMessager = clientMessager;
            _auth = auth;
        }
        public async Task<BaseResult_VM<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            request.Auth = _auth.GetDefaultAuth();
            var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<bool>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_UpdateUser");
            if (!methodResult.Success)
            {
                return new BaseResult_VM<bool>(false, methodResult.Code, "خطا در فراخوانی سرویس ویرایش کاربر", false);
            }

            return methodResult.Result;
        }
    }
}

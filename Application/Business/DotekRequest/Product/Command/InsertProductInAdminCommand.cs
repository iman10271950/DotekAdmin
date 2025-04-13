using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Business.DotekRequest.Product.ViewModel;
using Application.Business.DotekRequest.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.InterFaces.Services;
using Application.Common.Messager.Enums;
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.DotekRequest.Product.Command
{
    public class InsertProductInAdminCommand:IRequest<BaseResult_VM<bool>>
    {
        public OtherServicesAuth_VM Auth { get; set; }
        public Product_VM InputItem { get; set; }
        public List<InsertOtherProperty_VM> OtherProperties { get; set; }
    }
    public class InsertProductInAdminCommandHandler : IRequestHandler<InsertProductInAdminCommand, BaseResult_VM<bool>>
    {
        private readonly IClientMessager _clientMessager;
        private readonly IAuthHelper _auth;

        public InsertProductInAdminCommandHandler(IClientMessager clientMessager,IAuthHelper authHelper)
        {
            _clientMessager = clientMessager;
            _auth = authHelper;
        }
        public async Task<BaseResult_VM<bool>> Handle(InsertProductInAdminCommand request, CancellationToken cancellationToken)
        {
            request.Auth = _auth.GetDefaultAuth();
            var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<bool>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_InsertProductInAdmin");
            if (!methodResult.Success)
            {
                return new BaseResult_VM<bool>(false, methodResult.Code, "خطا در فراخوانی سرویس افزودن محصول");
            }

            return methodResult.Result;
        }
    }
}

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

namespace Application.Business.DotekRequest.Product.Query
{
    public class GetProductWithIdQuery:IRequest<BaseResult_VM<ProductInfo_VM>>
    {
        public long ProductId { get; set; }
        public OtherServicesAuth_VM Auth { get; set; }
    }
    public class GetProductWithIdQueryHandler : IRequestHandler<GetProductWithIdQuery, BaseResult_VM<ProductInfo_VM>>
    {
        private readonly IClientMessager _clientMessager;
        private readonly IAuthHelper _auth;

        public GetProductWithIdQueryHandler(IClientMessager clientMessager,IAuthHelper auth)
        {
            _clientMessager = clientMessager;
            _auth = auth;
        }
        public async Task<BaseResult_VM<ProductInfo_VM>> Handle(GetProductWithIdQuery request, CancellationToken cancellationToken)
        {
            request.Auth = _auth.GetDefaultAuth();
            var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<ProductInfo_VM>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_GetProductWithId");
            if (!methodResult.Success)
            {
                return new BaseResult_VM<ProductInfo_VM>(null, methodResult.Code, "خطا در فراخوانی سرویس دریافت محصول");
            }

            return methodResult.Result;
        }
    }
}

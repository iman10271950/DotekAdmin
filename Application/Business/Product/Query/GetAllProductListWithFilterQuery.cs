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

namespace Application.Business.Product.Query
{
    public class GetAllProductListWithFilterQuery : IRequest<BaseResult_VM<List<Product_VM>>>
    {
        public OtherServicesAuth_VM Auth { get; set; }
        public string? ProductName { get; set; }
    }
    public class GetAllProductListWithFilterQueryHandler : IRequestHandler<GetAllProductListWithFilterQuery, BaseResult_VM<List<Product_VM>>>
    {
        private readonly IClientMessager _clientMessager;
        private readonly IAuthHelper _auth;

        public GetAllProductListWithFilterQueryHandler(IClientMessager clientMessager,IAuthHelper auth)
        {
           _clientMessager = clientMessager;
           _auth = auth;
        }
        public async Task<BaseResult_VM<List<Product_VM>>> Handle(GetAllProductListWithFilterQuery request, CancellationToken cancellationToken)
        {
            request.Auth = _auth.GetDefaultAuth();
            var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<List<Product_VM>>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_GetAllProductListWithFilter");
            if (!methodResult.Success)
            {
                return new BaseResult_VM<List<Product_VM>>(null, methodResult.Code, "خطا در فراخوانی سرویس دریافت لیست محصولات");
            }

            return methodResult.Result;
        }
    }
}

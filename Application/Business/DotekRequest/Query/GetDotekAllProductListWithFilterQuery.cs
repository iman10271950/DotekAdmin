using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Business.DotekRequest.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.Messager.Enums;
using Application.Common.Models;
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.DotekRequest.Query
{
    public class GetDotekAllProductListWithFilterQuery : IRequest<BaseResult_VM<List<Product_VM>>>
    {
        public string? ProductName { get; set; }
    }
    public class GetDotekAllProductListWithFilterQueryHAndler : IRequestHandler<GetDotekAllProductListWithFilterQuery, BaseResult_VM<List<Product_VM>>>
    {
        private readonly IClientMessager _clientMessager;

        public GetDotekAllProductListWithFilterQueryHAndler(IClientMessager clientMessager)
        {
            _clientMessager = clientMessager;
        }
        public async Task<BaseResult_VM<List<Product_VM>>> Handle(GetDotekAllProductListWithFilterQuery request, CancellationToken cancellationToken)
        {
            var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<List<Product_VM>>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_GetAllProductListWithFilter");
            if (methodResult.Code == 103)
            {
                return methodResult.Result;
            }
            if (!methodResult.Success)
            {
                return new BaseResult_VM<List<Product_VM>>(null, methodResult.Code, "خطا در فراخوانی سرویس دریافت لیست محصولات", false);
            }

            return methodResult.Result;
        }
    }
}

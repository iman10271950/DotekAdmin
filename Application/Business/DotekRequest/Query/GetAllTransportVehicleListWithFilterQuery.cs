using Application.Business.DotekRequest.ViewModel;
using Application.Business.TransportVehicleBusiness.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.InterFaces.Services;
using Application.Common.Messager.Enums;
using Application.Common.Models;
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.DotekRequest.Query;

public class GetAllTransportVehicleListWithFilterQuery:IRequest<BaseResult_VM<PaginatedList<TransportVehicle_VM>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public string? DriverName { get; set; }
}
public class GetAllTransportVehicleListWithFilterQueryHandler:IRequestHandler<GetAllTransportVehicleListWithFilterQuery,BaseResult_VM<PaginatedList<TransportVehicle_VM>>>
{
    private readonly IClientMessager _clientMessager;
    private readonly IAuthHelper _auth;

    public GetAllTransportVehicleListWithFilterQueryHandler(IClientMessager clientMessager,IAuthHelper auth)
    {
        _clientMessager = clientMessager;
        _auth = auth;
    }
    public async Task<BaseResult_VM<PaginatedList<TransportVehicle_VM>>> Handle(GetAllTransportVehicleListWithFilterQuery request, CancellationToken cancellationToken)
    {
        var input = new GetAllTransportVehicleListWithFilterQueryInput
        {
            Auth = _auth.GetDefaultAuth(),
            DriverName = request.DriverName,
            PageNumber =  request.PageNumber,
            PageSize = request.PageSize,
          
        };

        var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<PaginatedList<TransportVehicle_VM>>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(input), "Admin_GetAllTransportVehicleListWithFilter");
          
       
        if (!methodResult.Success)
        {
            return new BaseResult_VM<PaginatedList<TransportVehicle_VM>>(null, methodResult.Code, "خطا در فراخوانی سرویس دزیافت لیست وسایل حمل", false);
        }

        return methodResult.Result;
    }

    private class GetAllTransportVehicleListWithFilterQueryInput
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? DriverName { get; set; }
        public OtherServicesAuth_VM Auth { get; set; }
    }
}
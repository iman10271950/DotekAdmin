using Application.Business.DotekRequest.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.InterFaces.Services;
using Application.Common.Messager.Enums;
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.DotekRequest.Query;

public class UpsertRequestTransportVehicleQuery:IRequest<BaseResult_VM<bool>>
{
    public long RequestId { get; set; }
    public long TransportVehicleId { get; set; }
  
}
public class UpsertRequestTransportVehicleQueryHandler:IRequestHandler<UpsertRequestTransportVehicleQuery,BaseResult_VM<bool>>
{
    private readonly IClientMessager _clientMessager;
    private readonly IAuthHelper _auth;

    public UpsertRequestTransportVehicleQueryHandler(IClientMessager clientMessager,IAuthHelper auth)
    {
        _clientMessager = clientMessager;
        _auth = auth;
    }
    public async Task<BaseResult_VM<bool>> Handle(UpsertRequestTransportVehicleQuery request, CancellationToken cancellationToken)
    {
        var input = new UpsertRequestTransportVehicleQueryInput
        {
            Auth = _auth.GetDefaultAuth(),
            IsAdminUser = true,
            RequestId = request.RequestId,
            SetDriverByAdmin = false, 
            TransportVehicleId = request.TransportVehicleId
          
        };

        var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<bool>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(input), "Admin_UpsertRequestTransportVehicle");
          
      
        if (!methodResult.Success)
        {
            return new BaseResult_VM<bool>(false, methodResult.Code, "خطا در فراخوانی سرویساافزودن وسیله حمل", false);
        }

        return methodResult.Result;
    }

    private class UpsertRequestTransportVehicleQueryInput
    {
        public long RequestId { get; set; }
        public long TransportVehicleId { get; set; }
        public bool SetDriverByAdmin { get; set; } = false;
        public bool IsAdminUser { get; set; } = false;
        public OtherServicesAuth_VM Auth { get; set; }
    }
}
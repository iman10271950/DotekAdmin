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

public class InsertTransportVehicleQuery: IRequest<BaseResult_VM<bool>>
{
    public TransportVehicle_VM Input { get; set; }
}
public class InsertTransportVehicleQuerHandler:IRequestHandler<InsertTransportVehicleQuery,BaseResult_VM<bool>>
{
    private readonly IClientMessager _clientMessager;
    private readonly IAuthHelper _auth;

    public InsertTransportVehicleQuerHandler(IClientMessager clientMessager,IAuthHelper auth)
    {
        _clientMessager = clientMessager;
        _auth = auth;
    }
    public async Task<BaseResult_VM<bool>> Handle(InsertTransportVehicleQuery request, CancellationToken cancellationToken)
    {
        var input = new InsertTransportVehicleQueryInput
        {
            Auth = _auth.GetDefaultAuth(),
            Input = request.Input,
            IsAdminUser = true,
          
        };

        var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<bool>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(input), "Admin_InsertTransportVehicle");
          
       
        if (!methodResult.Success)
        {
            return new BaseResult_VM<bool>(false, methodResult.Code, "خطا در فراخوانی سرویساافزودن وسیله حمل", false);
        }

        return methodResult.Result;
    }

    private class InsertTransportVehicleQueryInput
    {
        public OtherServicesAuth_VM Auth { get; set; }
        public TransportVehicle_VM Input { get; set; }
        public bool IsAdminUser { get; set; } = false;
    }
}
using Application.Business.DotekRequest.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.InterFaces.Services;
using Application.Common.Messager.Enums;
using Crypto.AES;
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.PaymentServices.Query;

public class ConfrimInvoicePayementQuery : IRequest<BaseResult_VM<bool>>
{
    public long InvoiceId { get; set; }
}

public class ConfrimInvoicePayementQueryHandler:IRequestHandler<ConfrimInvoicePayementQuery,BaseResult_VM<bool>>
{
    private readonly IClientMessager _clientMessager;
    private readonly IAuthHelper _authHelper;

    public ConfrimInvoicePayementQueryHandler(IClientMessager clientMessager,IAuthHelper authHelper)
    {
        _clientMessager = clientMessager;
        _authHelper = authHelper;
    }
    public async Task<BaseResult_VM<bool>> Handle(ConfrimInvoicePayementQuery request, CancellationToken cancellationToken)
    {
        var input = new ConfrimInvoicePayementQueryInput
        {
           Auth = _authHelper.GetDefaultAuth(),
           InvoiceId = request.InvoiceId,
        };
        var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<bool>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(input), "Admin_ConfrimInvoicePayement");
        if (methodResult.Code == 103)
        {
            return methodResult.Result;
        }
        return methodResult.Result;
        

    }

    private class ConfrimInvoicePayementQueryInput
    {
        public long InvoiceId { get; set; }
        public OtherServicesAuth_VM Auth { get; set; }
    }
}

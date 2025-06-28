using Application.Business.DotekUser.Query;
using Application.Business.PaymentServices.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.Messager.Enums;
using Application.Common.Models;
using Crypto.AES;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Application.Business.PaymentServices.Query;

public class GetPaymentDataNeedOperationListQuery:IRequest<BaseResult_VM<PaginatedList<PaymentHistory_VM>>>
{
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 5;
}
public class GetPaymentDataNeedOperationListQueryHandler:IRequestHandler<GetPaymentDataNeedOperationListQuery,BaseResult_VM<PaginatedList<PaymentHistory_VM>>>
{
    private readonly IClientMessager _clientMessager;
    private readonly IOptions<MessageSecuritySettings> _securityOptions;
    private readonly IMediator _mediator;

    public GetPaymentDataNeedOperationListQueryHandler(IClientMessager clientMessager,
        IOptions<MessageSecuritySettings> securityOptions,IMediator mediator)
    {
        _clientMessager = clientMessager;
        _securityOptions = securityOptions;
        _mediator = mediator;
    }
    public async Task<BaseResult_VM<PaginatedList<PaymentHistory_VM>>> Handle(GetPaymentDataNeedOperationListQuery request, CancellationToken cancellationToken)
    {
        var input = new GetPaymentDataNeedOperationListInput
        {
            CompanyId = _securityOptions.Value.CompanyCode,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        };
        string encryptedInput = AES.EncryptString(_securityOptions.Value.DPassword, JsonConvert.SerializeObject(input));

        var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<string>>(MicroServiceName.Payment,
            encryptedInput, "Company_GetPaymentDataNeedOperationList");

        if (!methodResult.Success)
        {
            return new BaseResult_VM<PaginatedList<PaymentHistory_VM>>(null, -1, "خطا در فراخوانی سرویس دریافت اطلاعات مالی", false);
        }

        if (methodResult.Result.Code != 0)
        {
            return new BaseResult_VM<PaginatedList<PaymentHistory_VM>>(null, methodResult.Result.Code, methodResult.Result.Message, false);
        }

        var decryptResponse = AES.DecryptString(_securityOptions.Value.DPassword, methodResult.Result.Result);
        if (decryptResponse == null)
        {
            return new BaseResult_VM<PaginatedList<PaymentHistory_VM>>(null, -2, "خطا در فراخوانی سرویس دریافت اطلاعات مالی", false);
        }

        var result = JsonConvert.DeserializeObject<PaginatedList<PaymentHistory_VM>>(decryptResponse);

        foreach (var item in result.Items)
        {
            var user = await _mediator.Send(new GetUserInformationWithIdInAdminQuery
                { NationalCode = item.UserNationalCode });
            if (user.Code == 0)
            {
                item.User = user.Result;
                
            }
        } 
        return new BaseResult_VM<PaginatedList<PaymentHistory_VM>>
        {
            Result = result,
            Code = 0,
            Message = "با موفقیت انجام شد"
        };
    }

    private class GetPaymentDataNeedOperationListInput
    {
        public long CompanyId { get; set; }
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 5;
    }
}

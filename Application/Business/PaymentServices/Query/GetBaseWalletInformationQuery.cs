using Application.Business.PaymentServices.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.Messager.Enums;
using MediatR;
using Microsoft.Extensions.Options;
using Crypto.AES;
using Newtonsoft.Json;

namespace Application.Business.PaymentServices.Query;

public class GetBaseWalletInformationQuery:IRequest<BaseResult_VM<Wallet_VM>>
{
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 5;
}

public class
    GetBaseWalletInformationQueryHandler : IRequestHandler<GetBaseWalletInformationQuery, BaseResult_VM<Wallet_VM>>
{
    private readonly IClientMessager _clientMessager;
    private readonly IOptions<MessageSecuritySettings> _securityOptions;

    public GetBaseWalletInformationQueryHandler(IClientMessager clientMessager,
        IOptions<MessageSecuritySettings> securityOptions)
    {
        _clientMessager = clientMessager;
        _securityOptions = securityOptions;
    }

    public async Task<BaseResult_VM<Wallet_VM>> Handle(GetBaseWalletInformationQuery request,
        CancellationToken cancellationToken)
    {



        var input = new GetBaseWalletInformationInput
        {
            CompanyId = _securityOptions.Value.CompanyCode,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        };
        string encryptedInput = AES.EncryptString(_securityOptions.Value.DPassword, JsonConvert.SerializeObject(input));

        var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<string>>(MicroServiceName.Payment,
            encryptedInput, "Company_GetCompanyWalletInformation");

        if (!methodResult.Success)
        {
            return new BaseResult_VM<Wallet_VM>(null, -1, "خطا در فراخوانی سرویس دریافت اطلاعات کیف پول", false);
        }

        if (methodResult.Result.Code != 0)
        {
            return new BaseResult_VM<Wallet_VM>(null, methodResult.Result.Code, methodResult.Result.Message, false);
        }

        var decryptResponse = AES.DecryptString(_securityOptions.Value.DPassword, methodResult.Result.Result);
        if (decryptResponse == null)
        {
            return new BaseResult_VM<Wallet_VM>(null, -2, "خطا در فراخوانی سرویس دریافت اطلاعات کیف پول", false);
        }

        var result = JsonConvert.DeserializeObject<Wallet_VM>(decryptResponse);
        return new BaseResult_VM<Wallet_VM>
        {
            Result = result,
            Code = 0,
            Message = "با موفقیت انجام شد"
        };
    }

    private class GetBaseWalletInformationInput
    {
        public long CompanyId { get; set; }
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 5;
    }
}
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.Messager.Enums;
using Crypto.AES;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Application.Business.PaymentServices.Query;

public class ConfrimUserWithdrawRequestCommand:IRequest<BaseResult_VM<bool>>
{
    public long WithdrawRequestId { get; set; }
    public string PaymentTraceCode { get; set; }
}
public class ConfrimUserWithdrawRequestCommandHandler:IRequestHandler<ConfrimUserWithdrawRequestCommand,BaseResult_VM<bool>>
{
    private readonly IClientMessager _clientMessager;
    private readonly IOptions<MessageSecuritySettings> _securityOptions;

    public ConfrimUserWithdrawRequestCommandHandler(IClientMessager clientMessager,
        IOptions<MessageSecuritySettings> securityOptions)
    {
        _clientMessager = clientMessager;
        _securityOptions = securityOptions;
    }
    public async Task<BaseResult_VM<bool>> Handle(ConfrimUserWithdrawRequestCommand request, CancellationToken cancellationToken)
    {
       
        var input = new ConfrimUserWithdrawRequestInput
        {
          PaymentTraceCode = request.PaymentTraceCode,
          WithdrawRequestId = request.WithdrawRequestId 
        };
        string encryptedInput = AES.EncryptString(_securityOptions.Value.DPassword, JsonConvert.SerializeObject(input));

        var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<string>>(MicroServiceName.Payment,
            encryptedInput, "Company_ConfrimUserWithdrawRequest");

        if (!methodResult.Success)
        {
            return new BaseResult_VM<bool>(false, -1, "خطا در فراخوانی سرویس تایید انتقال ", false);
        }

        if (methodResult.Result.Code != 0)
        {
            return new BaseResult_VM<bool>(false, methodResult.Result.Code, methodResult.Result.Message, false);
        }


        return new BaseResult_VM<bool>
        {
            Result = true,
            Code = 0,
            Message = "با موفقیت انجام شد"
        };
    }

    private class ConfrimUserWithdrawRequestInput
    {
        public long WithdrawRequestId { get; set; }
        public string PaymentTraceCode { get; set; }
    }
}
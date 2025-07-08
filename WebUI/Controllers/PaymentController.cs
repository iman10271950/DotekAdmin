using Application.Business.PaymentServices.Query;
using Application.Common.Attributes;
using Application.Common.Auth;
using Domain.Enums;
using Domain.Enums.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class PaymentController:ApiControllerBase
{
    public PaymentController(IMediator  mediator):base((mediator))
    {
        
    }
    /// <summary>
    /// دریافت اطلاعات کیف پول اصلی
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPost]
    [DotekAuthorize(AuthorizeEnum.Payment_GetBaseWalletInformation)]
    [DotekLog(AdminServices.Payment, AdminMethods.Payment_GetBaseWalletInformation)]
    public async Task<IActionResult> GetBaseWalletInformation([FromBody] GetBaseWalletInformationQuery command)
    {
        return Ok(await Mediator.Send(command));
    }
    /// <summary>
    /// دریافت لیست درخواست های انتقال 
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPost]
    [DotekAuthorize(AuthorizeEnum.Payment_GetPaymentDataNeedOperationList)]
    [DotekLog(AdminServices.Payment, AdminMethods.Payment_GetPaymentDataNeedOperationList)]
    public async Task<IActionResult> GetPaymentDataNeedOperationList([FromBody] GetPaymentDataNeedOperationListQuery command)
    {
        return Ok(await Mediator.Send(command));
    }
    /// <summary>
    /// تایید  درخواست  انتقال 
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPost]
    [DotekAuthorize(AuthorizeEnum.Payment_ConfrimUserWithdrawRequest)]
    [DotekLog(AdminServices.Payment, AdminMethods.Payment_ConfrimUserWithdrawRequest)]
    public async Task<IActionResult> ConfrimUserWithdrawRequest([FromBody] ConfrimUserWithdrawRequestCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
}
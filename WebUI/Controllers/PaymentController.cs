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
    [DotekAuthorize(AuthorizeEnum.Dotek_GetAllRoles)]
    [DotekLog(AdminServices.Payment, AdminMethods.Payment_GetBaseWalletInformation)]
    [HttpPost]
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
    public async Task<IActionResult> GetPaymentDataNeedOperationList([FromBody] GetPaymentDataNeedOperationListQuery command)
    {
        return Ok(await Mediator.Send(command));
    }
    /// <summary>
    /// دریافت لیست درخواست های انتقال 
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPost]
    [DotekLog(AdminServices.Payment, AdminMethods.Payment_ConfrimUserWithdrawRequest)]
    public async Task<IActionResult> ConfrimUserWithdrawRequest([FromBody] ConfrimUserWithdrawRequestCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
}
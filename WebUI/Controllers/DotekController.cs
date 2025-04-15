using Application.Business.Common.ViewModel;
using Application.Business.DotekRequest.Query;
using Application.Business.DotekRequest.ViewModel;
using Application.Business.DotekUser.Command;
using Application.Business.DotekUser.Query;
using Application.Business.Product.Command;
using Application.Business.Product.Query;
using Application.Common.Attributes;
using Application.Common.Auth;
using Domain.Entities.Log;
using Domain.Enums.Auth;
using ICSharpCode.Decompiler.TypeSystem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class DotekController1 : ApiControllerBase
    {
        public DotekController1(IMediator mediator):base(mediator)
        {
            
        }
        /// <summary>
        /// دریافت لیست کاربران
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        //[DotekAuthorize(AuthorizeEnum.Request_GetCurrentUserRequestList)]
        [DotekLog(AdminServices.Request, AdminMethods.Request_GetAllProductListWithFilter)]
        [ProducesResponseType(typeof(AuthenticatedResponse_VM), 0)]
        [ProducesResponseType(typeof(List<Product_VM>), 1)]
        public async Task<IActionResult> GetAllUserForAdmin([FromBody] GetAllUserForAdminQuery command)
        {
            return Ok(await Mediator.Send(command));
        }
       
        /// <summary>
        /// دریافت لیست نقش ها
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetAllRoles([FromBody] GetAllRolesQuery command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// دریافت اطلاعات کاربر با شناسه
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetUserInformationWithIdInAdmin([FromBody] GetUserInformationWithIdInAdminQuery command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// ویرایش کاربر
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// ویرایش نقش
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateRolleInAdmin([FromBody] UpdateRolleInAdminCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// حذف نقش
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteDotekRolle([FromBody] DeleteDotekRolleCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// غیر فعالسازی اکانت کاربر
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InactiveDotekUser([FromBody] InactiveDotekUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// افزودن محصول
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InsertProductInAdmin([FromBody] InsertProductInAdminCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// دریافت اطلاعات یک محصول با شناسه محصول
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetProductWithId([FromBody] GetProductWithIdQuery command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// حذف محصول
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// دریافت لیست محصولات
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetAllProductListWithFilter([FromBody] GetAllProductListWithFilterQuery command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// دریافت لیست درخواست ها
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetlAllDotekRequest([FromBody] GetlAllDotekRequestQuery command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        ///  به روز رسانی درخواست
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateDotekReques([FromBody] UpdateDotekRequesQtuery command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        ///  غیرفعال سازی درخواست
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InActiveDotekRequestStatus([FromBody] InActiveDotekRequestStatusQuery command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        ///  پیش نیاز های لیست درخواست ها
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PrepaireRequestList([FromBody] PrepaireRequestListQuery command)
        {
            return Ok(await Mediator.Send(command));
        }
        

    }
}

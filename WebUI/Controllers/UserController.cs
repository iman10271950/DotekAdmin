using Application.Business.Auth.Session.Command;
using Application.Business.Auth.Session.Query;
using Application.Business.Auth.User.Command;
using Application.Business.Auth.User.Query;
using Application.Business.Common.ViewModel;
using Application.Business.Product.Command;
using Application.Business.Product.Query;
using Application.Common.Attributes;
using Domain.Entities.Log;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class UserController : ApiControllerBase
    {
        public UserController(IMediator mediator) : base(mediator)
        {

        }
        /// <summary>
        /// بررسی وجود کاربر
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [DotekLog(AdminServices.User, AdminMethods.User_ExistUser)]
        [PointerId("PhoneNumber")]
        public async Task<IActionResult> ExistUser([FromBody] ExistUserQuery command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// ساخت اکانت
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [SensitiveData(new string[] { "Password" })]
        [DotekLog(AdminServices.User, AdminMethods.User_CreateAccount)]
        [PointerId("NationalCode")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// ورود
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [DotekLog(AdminServices.User, AdminMethods.User_Login)]
        [PointerId("PhoneNumber")]
        [ProducesResponseType(typeof(AuthenticatedResponse_VM), 0)]
        [ProducesResponseType(typeof(List<Session_VM>), 1)]
        public async Task<IActionResult> Login([FromBody] LoginWithOPTCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    
    
        /// <summary>
        /// لاگ اوت
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Logout(LogoutUserQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
        /// <summary>
        /// بروزرسانی کاربر ادمین
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [DotekLog(AdminServices.User, AdminMethods.User_UpdateUserCommand)]
        [PointerId("Input.Id")]
        public async Task<IActionResult> UpdateAdminUser([FromBody] UpdateAdminUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        
        #region Session
        /// <summary>
        /// غیر فعال کردن نشست فعال کاربر
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> KillSession([FromQuery] KillSessionQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// رفرش کردن توکن
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <response code="1">توکن منقضی شده است. کاربر باید به صفحه لاگین هدایت شود</response>
        /// <response code="0">توکن و رفرش توکن جدید با موفقیت ساخته شده است</response>
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] SessionRefreshTokenCommand query)
        {
            return Ok(await Mediator.Send(query));
        }
        #endregion
    }


}
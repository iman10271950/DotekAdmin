using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums.Auth
{
    public enum AuthorizeEnum
    {
        //    [Description("دریافت آدرس با کد پستی")]
        //    Request_GetAddressFromPostalCode=3,
        //    [Description("دریافت لیست درخواست های خرید مطابق")]
        //    Request_GetSimilarRequestList=4,
        //    [Description("افزودن درخواست")]
        //    Request_InsertRequest=5,
        //    [Description("دریافت پیش نیاز های افزودن درخواست")]
        //    Request_PrepaireInsertRequest=6,
        //    [Description("دریافت اطلاعات درخواست با شناسه")]
        //    Request_GetRequestInformationWithID=7,
        [Description("دریافت لیست درخواست ها")]
        Request_GetCurrentUserRequestList = 1,
        [Description("دریافت لیست نقش ها")]
        Dotek_GetAllRoles = 2,
        [Description("دریافت اطلاعات کاربر با شناسه")]
        Dotek_GetUserInformationWithIdInAdmin = 3,
        [Description("ویرایش کاربر Dotek")]
        Dotek_UpdateUser = 4,
        [Description("ویرایش نقش")]
        Dotek_UpdateRolleInAdmin = 5,
        [Description("حذف نقش")]
        Dotek_DeleteDotekRolle = 6,
        [Description("غیر فعالسازی اکانت کاربر")]
        Dotek_InactiveDotekUser = 7,
        [Description("افزودن محصول")]
        Dotek_InsertProductInAdmin = 8,
        [Description("دریافت اطلاعات یک محصول با شناسه محصول")]
        Dotek_GetProductWithId = 9,
        [Description("حذف محصول")]
        Dotek_DeleteProduct = 10,
        [Description("دریافت لیست محصولات")]
        Dotek_GetAllProductListWithFilter = 11,
        [Description("")]
        Dotek_GetlAllDotekRequest=12,
        //[Description("دریافت لیست محصولات")]
        //Request_GetAllProductListWithFilter=9,

    }
}

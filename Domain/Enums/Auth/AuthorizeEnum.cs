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
    //Dotek
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
        [Description("دریافت لیست کاربران")]
        Dotek_GetAllUserForAdmin=10005,
        [Description(" ویرایش وضعیت دسته کاربران")]
        Dotek_UpdateUsersStatus=10006,
        [Description(" دریافت لیست درخواست ها")]
        Dotek_GetlAllDotekRequest=10007,
        [Description(" به روز رسانی درخواست")]
        Dotek_UpdateDotekReques=10008,
        [Description("تغییر وضعیت دسته ای درخواست")]
        Dotek_UpdateDotekRequestStatus=10009,
        [Description("پیش نیاز های لیست درخواست ها")]
        Dotek_PrepaireRequestList=10010,
        [Description("پیش نیاز های سرویس های دوتک")]
        Dotek_PrepaireDotekServices=10011,
        [Description(" دزیافت لیست فاکتور ها با پیجینگ")]
        Dotek_GetAllInvoiceList=10012,
        
        
        
        
        //Payment
        [Description("دریافت اطلاعات کیف پول اصلی")]
        Payment_GetBaseWalletInformation=10002,
        [Description("دریافت لیست درخواست های انتقال ")]
        Payment_GetPaymentDataNeedOperationList=10003,
        [Description("تایید  درخواست  انتقال ")]
        Payment_ConfrimUserWithdrawRequest=10004,
        [Description("تایید فاکتور برای انتقال وجه")]
        Payment_ConfrimInvoicePayement=10013,
  
    

    }
}

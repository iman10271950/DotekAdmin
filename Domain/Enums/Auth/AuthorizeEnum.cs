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
        [Description("دریافت آدرس با کد پستی")]
        Request_GetAddressFromPostalCode=3,
        [Description("دریافت لیست درخواست های خرید مطابق")]
        Request_GetSimilarRequestList=4,
        [Description("افزودن درخواست")]
        Request_InsertRequest=5,
        [Description("دریافت پیش نیاز های افزودن درخواست")]
        Request_PrepaireInsertRequest=6,
        [Description("دریافت اطلاعات درخواست با شناسه")]
        Request_GetRequestInformationWithID=7,
        [Description("دریافت لیست درخواست ها")]
        Request_GetCurrentUserRequestList=8,
        [Description("دریافت لیست محصولات")]
        Request_GetAllProductListWithFilter=9,

    }
}

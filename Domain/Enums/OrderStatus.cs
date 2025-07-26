using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum OrderStatus
    {
        /// <summary>
        /// نامعلوم
        /// </summary>
        [Description("نامعلوم")]
        Unknown = 0,
        /// <summary>
        /// منتظر تایید ادمین
        /// </summary>
        [Description("منتظر تایید ادمین")]
        WaitingForAdminApproval = 1,
        /// <summary>
        /// ثبت شده
        /// </summary>
        [Description("فعال")]
        Active = 2,
     
        /// <summary>
        /// منتظر پرداخت
        /// </summary>
        [Description("منتظر پرداخت")]
        AwaitingPayment = 3,
        /// <summary>
        /// منتظر وارد کردن اطلاعات وسیله حمل
        /// </summary>
        [Description("منتظر وارد کردن اطلاعات وسیله حمل")]
        WaitingVehicleInformation=4,
        [Description("منتظر وارد کردن اطلاعات وسیله حمل توسط ادمین")]
        WaitingAdminVehicleInformation=5,
        /// <summary>
        /// بارگیری محصول
        /// </summary>
        [Description("بارگیری محصول")]
        LoadingProduct=6,
        /// <summary>
        /// منتظر تایید فاکتور
        /// </summary>
        [Description("منتظر تایید فاکتور")]
        WaitingForInvoiceConfirmation = 7,
        /// <summary>
        ///تسویه حساب
        /// </summary>
        [Description("تسویه حساب")]
        AccountSettlement = 8,
        /// <summary>
        /// 
        /// </summary>
        [Description("اتمام معامله با موفقیت")]
        SuccessfullyDone=9,
        /// <summary>
        /// غیرفعال
        /// </summary>
        [Description("غیرفعال")]
        inActive =10,

    }
}

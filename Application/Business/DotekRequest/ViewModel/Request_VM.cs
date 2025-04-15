using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extentions;
using DocumentFormat.OpenXml.Bibliography;

namespace Application.Business.DotekRequest.ViewModel
{
     public class Request_VM 
    {
        public long Id { get; set; }
        [Description("شناسه کاربر")]
        public long UserId { get; set; }

        [Description("قیمت")]
        public decimal Price { get; set; }

        [Description("شناسه محصول")]
        public long ProductId { get; set; }

        [Description("شناسه واحد محصول")]
        public long? ProductUnitId { get; set; }

        [Description("وضعیت سفارش")]
        public int Status { get; set; } 
        public string OrderRequeststatusDesc { get; set; }

        [Description("نوع درخواست")]
        public int RequestType { get; set; } 
        public string RequestTypeDesc { get; set; }

        [Description("وزن")]
        public decimal Weight { get; set; }

        [Description("نوع پرداخت")]
        public int PymentType { get; set; } 
        public string PymentTypeDesc { get; set; }

        [Description("مقدار")]
        public decimal Quantity { get; set; }

        [Description("تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
        public string CreateDateShamsi { get; set; }

        [Description("تاریخ انقضا")]
        public DateTime DateExpire { get; set; }
        public string DateExpireShamsi { get; set;; }
        public long CityId { get; set; }
        public long ProviceId { get; set; }
    }
}

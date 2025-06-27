using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Business.Auth.User.ViewModel;
using Application.Business.Product.ViewModel;
using Application.Business.TransportVehicleBusiness.ViewModel;
using Application.Common.Extentions;
using DocumentFormat.OpenXml.Bibliography;

namespace Application.Business.DotekRequest.ViewModel
{
     public class Request_VM 
    {
        public long Id { get; set; }
        [Description("کد یکتای درخواست")]
        public string? RequestTraceCode { get; set; }
        [Description("شناسه کاربر")]
        public long UserId { get; set; }

        [Description("قیمت")] 
        public decimal? Price { get; set; } 

        [Description("شناسه درخواست پیوست شده")]
        public long? AttachedRequestId { get; set; }
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

        [Description("تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
        public string CreateDateShamsi { get; set; }

        [Description("تاریخ انقضا")]
        public DateTime DateExpire { get; set; }
        public string DateExpireShamsi { get; set; }
        public bool IsrequestEditable { get; set; } = false;
        public User_VM? user { get; set; }
        public List<ViewOtherProperty_VM>? ViewOtherProperties { get; set; }
        public ProductUnit_VM? ProductUnit { get; set; }
        public Product_VM? Product { get; set; }
        [Description("واحد محصول")]
        public Request_VM? AttachedRequest { get; set; }
        public TransportVehicle_VM? TransportVehicle { get; set; }
    }
}

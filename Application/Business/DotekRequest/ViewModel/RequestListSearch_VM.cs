using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;

namespace Application.Business.DotekRequest.ViewModel
{
    public class RequestListSearch_VM
    {
        [Description("قیمت از")]
        public decimal? PriceFrom { get; set; }
        [Description("قیمت تا")]
        public decimal? ToPrice { get; set; }
        [Description("شناسه محصول")]
        public long? ProductId { get; set; }
        [Description("وضعیت درخواست")]
        public List<int>? Status { get; set; }
        [Description("نوع درخواست")]
        public int? RequestType { get; set; }
        [Description("وزن از")]
        public decimal? WeightFrom { get; set; }
        [Description("وزن تا")]
        public decimal? ToWeight { get; set; }
        [Description("نوع پرداخت")]
        public int? PymentType { get; set; }
        [Description("تاریخ از")]
        public string? DateFrom { get; set; }
        [Description("تاریخ تا")]
        public string? ToDate { get; set; }
        [Description("نوع تاریخ")]
        public int? DateType { get; set; }
        [Description("مقدار از")]
        public decimal? QuantityFrom { get; set; }
        [Description("مقدار تا")]
        public decimal? ToQuantity { get; set; }
        [Description("لیست شهر ها")]
        public List<int>? CityIds { get; set; }
        [Description("لیست استان ها")]
        public List<int>? ProviceIds { get; set; }
    }
}

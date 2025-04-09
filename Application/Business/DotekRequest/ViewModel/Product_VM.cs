using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.DotekRequest.ViewModel
{
    public class Product_VM 
    {
        [Description("شناسه")]
        public long Id { get; set; }

        [Description("نام")]
        public string Name { get; set; }

        [Description("نوع واحد محصول")]
        public int ProductUnit { get; set; } 

        [Description("توضیحات واحد محصول")]
        public string ProductUnitDesc { get; set; }

        [Description("واحد اندازه‌گیری")]
        public int MeasurementUnit { get; set; }

        [Description("توضیحات واحد اندازه‌گیری")]
        public string MeasurementUnitDesc { get; set; }
    }
}

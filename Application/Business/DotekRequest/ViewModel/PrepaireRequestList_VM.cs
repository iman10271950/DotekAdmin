using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.BaseEntities;

namespace Application.Business.DotekRequest.ViewModel
{
    public class PrepaireRequestList_VM
    {
        public List<BaseEnum_VM> DateType { get; set; }
        public List<BaseEnum_VM> OrderStatus { get; set; }
        public List<BaseEnum_VM> RequestType { get; set; }
        public List<BaseEnum_VM> PymentType { get; set; }
        public List<BaseEnum_VM> CityIds { get; set; }
        public List<BaseEnum_VM> ProviceIds { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Business.DotekRequest.Product.ViewModel
{
    public class InsertOtherProperty_VM
    {
        public string Name { get; set; }
        public OtherPropertyFieldsType OtherPropertyFieldsType { get; set; }
    }
}

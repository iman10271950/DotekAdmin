using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mapping;

using Domain.Enums;

namespace Application.Business.DotekRequest.ViewModel
{
    public class ViewOtherProperty_VM
    {
        public long Id { get; set; }
        public string FieldName { get; set; }
        public string FildValue { get; set; }
        public int? OtherPropertyType { get; set; }
    }
}

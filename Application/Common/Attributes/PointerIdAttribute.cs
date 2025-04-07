using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Attributes
{
    public class PointerIdAttribute : Attribute
    {
        public string PointerId { get; set; }
        public PointerIdAttribute(string pointerId)
        {
            PointerId = pointerId;
        }
    }
}

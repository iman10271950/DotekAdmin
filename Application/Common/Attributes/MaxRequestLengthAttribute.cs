using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Attributes
{
    public class MaxRequestLengthAttribute : Attribute
    {
        public int MaxRequestLength { get; set; }

        public MaxRequestLengthAttribute(int maxRequestLength)
        {
            MaxRequestLength = maxRequestLength;
        }
    }
}

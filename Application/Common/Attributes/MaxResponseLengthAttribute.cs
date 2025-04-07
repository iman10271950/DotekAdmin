using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Attributes
{
    public class MaxResponseLengthAttribute : Attribute
    {
        public int MaxResponseLength { get; set; }

        public MaxResponseLengthAttribute(int maxResponseLength)
        {
            MaxResponseLength = maxResponseLength;
        }
    }
}

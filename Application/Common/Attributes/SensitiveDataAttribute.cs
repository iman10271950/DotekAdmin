using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Attributes
{
    public class SensitiveDataAttribute : Attribute
    {
        public string[] SensitiveData { get; set; }
        public SensitiveDataAttribute(string[] sensitiveData)
        {
            SensitiveData = sensitiveData;
        }
    }
}

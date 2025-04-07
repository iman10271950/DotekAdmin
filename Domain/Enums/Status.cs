using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum Status
    {
        [Description("فعال")]
        Active = 1,

        [Description("غیرفعال")]
        Inactive = 2,
    }
}

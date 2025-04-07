using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums.Auth
{
    public enum UserStatus
    {
        [Description("فعال")]
        Active = 1,

        [Description("غیرفعال")]
        Inactive = 2,

        [Description("منتظر تایید ادمین")]
        PendingAcceptAdmin = 3,
    }
}

using System.ComponentModel;

namespace Domain.Enums;

public enum UserPaymentDataStatus
{
    Noun=0, 
    [Description("عملیات  موفق")]
    Active = 1,

    [Description("عملیات دچار مشکل شده است")]
    Inactive = 2,
    [Description("نیاز به عملیات")]
    NeedOperation=3,
    
}
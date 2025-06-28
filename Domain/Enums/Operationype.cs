using System.ComponentModel;

namespace Domain.Enums;

public enum Operationype
{
    /// <summary>
    /// واریز
    /// </summary>
    [Description("واریز")]
    Deposit=1,
    /// <summary>
    /// برداشت
    /// </summary>
    [Description("برداشت")]
    Withdraw=2,
    
    
}
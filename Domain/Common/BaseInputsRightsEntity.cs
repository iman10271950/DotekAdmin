using System.ComponentModel;

namespace Domain.Common;

public class BaseInputsRightsEntity:BaseAuditableEntity
{
    [Description("آیدی کالا")]
    public long GoodsId { get; set; }

    [Description("حقوق ورودی")]
    public decimal InputsRights { get; set; }

    [Description("مالیات بر ارزش افزوده")]
    public decimal TaxValue { get; set; }

    [Description("عوارض")]
    public decimal TollValue { get; set; }

    [Description("نخفیف مالیات")]
    public decimal TaxDiscount { get; set; }

    [Description("تخفیف عوارض")]
    public decimal TollDiscount { get; set; }
}

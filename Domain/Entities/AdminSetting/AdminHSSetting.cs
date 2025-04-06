using Domain.Common;
using System.ComponentModel;

namespace Domain.Entities.AdminSetting;

public class AdminHSSetting : BaseAuditableEntity
{
    [Description("کد تعرفه")]
    public string HSCode { get; set; }
    [Description("رویه فرعی")]
    public decimal? ImportQauntityTolerance { get; set; }
    [Description("میزان درصد تلورانس مقدار وزن خالص واردات قطعی")]
    public decimal? ImportNetWeightTolerance { get; set; }
    [Description("میزان درصد تلورانس مقدار وزن ناخالص واردات قطعی")]
    public decimal? ImportGrossWeightTolerance { get; set; }
    [Description("میزان  تلورانس گواهی بازرسی")]
    public decimal? InspectionCertificateTolerance { get; set; }
    [Description("لزوم چک شدن تعداد مقدار کالای گواهی بازرسی")]
    public bool? InspectionCertificateQuantity { get; set; }
    [Description("میزان تلورانس گواهی تولید")]
    public decimal? ProductionCertificateTolerance { get; set; }
    [Description("لزوم چک شدن تعداد مقدار کالای گواهی تولید")]
    public bool? ProductionCertificateQuantity { get; set; }
    [Description("لزوم چک شدن تعداد و مقدار و بسته قبض انبار")]
    public bool? MatchingWarehouseReceiptQuantityAndPackage { get; set; }
    [Description("لزوم چک شدن تعداد و مقدار و بسته بارنامه")]
    public bool? MatchingBillOfLadingQuantityAndPackage { get; set; }
    [Description("میزان درصد تلورانس مقدار وزن ناخالص بارنامه")]
    public decimal? BillOfLadingGrossWeightTolerance { get; set; }
    [Description("لزوم چک شدن کالای اظهارنامه ارزش")]
    public bool? ValueDeclarationGoods { get; set; }
    [Description("لزوم افزودن قرارداد بورسی")]
    public bool? NeedBourseContract { get; set; }
}

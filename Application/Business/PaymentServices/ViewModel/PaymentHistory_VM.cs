using Application.Business.DotekRequest.ViewModel;
using Application.Business.DotekUser.VieModel;
using Application.Common.Extentions;
using Domain.Enums;

namespace Application.Business.PaymentServices.ViewModel;

public class PaymentHistory_VM
{
    public long Id { get; set; }
    public long WalletId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreateDate { get; set; }
    public Operationype OperationType { get; set; }
    public string OperationTypeDesc { get=>OperationType==null ? "" :OperationType.GetDescription(); }
    public string? TrackingCode { get; set; }
    public UserPaymentDataStatus Status { get; set; }
    public string? UserNationalCode { get; set; }
    public string StatusDesc
    {
        get => Status == null ? "" : Status.GetDescription();
    }
    public int Provider { get; set; }
    public string ProviderDesc { get; set; }
    public DotekUser_VM? User { get; set; }
}
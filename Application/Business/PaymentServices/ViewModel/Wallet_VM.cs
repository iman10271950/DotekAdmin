using Application.Common.Models;

namespace Application.Business.PaymentServices.ViewModel;

public class Wallet_VM
{
    public decimal Balance { get; set; }
    public DateTime Create { get; set; }
    public DateTime LastUpdated { get; set; }
    public PaginatedList<PaymentHistory_VM> PaymentDataHistory { get; set; }
     public PaginatedList<PaymentHistory_VM> OtherUserPaymentDataHistory { get; set; }
}
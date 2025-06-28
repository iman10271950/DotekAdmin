namespace Application.Business.PaymentServices.ViewModel;

public class Wallet_VM
{
    public decimal Balance { get; set; }
    public DateTime Create { get; set; }
    public DateTime LastUpdated { get; set; }
    public List<PaymentHistory_VM> PaymentDataHistory { get; set; }
}
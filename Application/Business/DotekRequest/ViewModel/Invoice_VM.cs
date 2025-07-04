using Application.Business.DotekRequest.ViewModel;

namespace Application.Business.DotekRequest.ViewModel
{
    public class Invoice_VM
    {
        public string TraceCode { get; set; }
        public string SellerName { get; set; }
        public string BuyerName { get; set; }
        public string PruductName { get; set; }
        public string ProductUnitAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string DriverName { get; set; }
        public string plateNumber { get; set; }
        public string CreateDate { get; set; }
        public Request_VM RequestInformation { get; set; }

    }
}
using System.Numerics;

namespace SoftSolutions.Models
{
    public class Order: BaseModel
    {
        public int ServiceRequestId { get; set; }
        public double? TotalAmount { get; set; }
        public decimal? CommissionPercentage { get; set; }
        public double? CommissionAmount { get; set; }
        public double? ProviderEarning { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
        public Review Review { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Shared.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }

        public string? ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser? ApplicationUser { get; set; }
        public Guid? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        [ValidateNever]

        [DisplayName("Customer")]
        public Customer? Customer { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new();
        public string ContactNumber { get; set; }
        public string? AddressLine { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? Carrier { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal ServiceFee { get; set; } = 0;
        public decimal DeliveryFee { get; set; } = 0;
        public string? PaymentMode { get; set; }
        public double OrderTotal { get; set; }
        public double? AmountTendered { get; set; }
        public string? OfficialReceipt { get; set; }
        public string? SalesChannel { get; set; }
        public string? StoreName { get; set; }
        public DateTime? ProcessDate { get; set; } = DateTime.UtcNow;
        public DateTime? ShippingDate { get; set; } = DateTime.UtcNow;
        public DateTime CompletionDate { get; set; } = DateTime.UtcNow;
        public DateTime? CancellationDate { get; set; } = DateTime.UtcNow;
        public DateTime? RefundDate { get; set; } = DateTime.UtcNow;
        public string? ProcessedBy { get; set; }
        public string? CompletedBy { get; set; }
        public string? CancelledBy { get; set; }
        public string? RefundedBy { get; set; }
    }

    public class Discount
    {
        public string DiscountType { get; set; } = string.Empty;
        public double DiscountAmount { get; set; } = 0;
        public double TotalDiscount { get; set; } = 0;
        public double DiscountedPrice { get; set; } = 0;
    }

    public class DiscountModel
    {
        public string Percent { get; set; } = "percent";
        public string Amount { get; set; } = "amount"; 
    }
}

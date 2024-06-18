using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ABC.Shared.Models
{
    public class PurchaseOrder
    {
        [Key]
        public int Id { get; set; }
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
		public int SupplierId { get; set; }
		public Supplier Supplier { get; set; }
		public int StoreId { get; set; }
		public Store Store { get; set; }
		
		public List<PurchaseOrderItem> PurchasedProducts { get; set; }
		[Required]
		public string Status { get; set; }
		[Required]
		public long ContactNumber { get; set; }
		[Required]
		public string ContactPerson { get; set; }
		[Required]
		public string PaymentTerm { get; set; }
		[Required]
		public DateOnly DeliveryDate { get; set; }
		public double OrderTotal { get; set; }
    }

	public class PurchaseOrderItem
	{
		[Key]
		public int Id { get; set; }

		public int ProductId { get; set; }
		public Product Product { get; set; }

		public int PurchaseOrderId { get; set; }
		public PurchaseOrder PurchaseOrder { get; set; }
		[DisplayName("Cost")]
		public double Cost { get; set; }

		[DisplayName("Quantity")]
		public int Quantity { get; set; }
		public double SubTotal { get; set; }
	}
}








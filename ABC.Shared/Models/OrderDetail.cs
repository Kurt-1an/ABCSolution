using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Shared.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        [Required]
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        public double? Charge { get; set; }
        public double? Discount { get; set; }
        public int Count {  get; set; }
        //public double Price {  get; set; }
        public double RetailPrice {  get; set; }
        public double CostPrice {  get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
		public string? Remark { get; set; }
		public int? remarkQty { get; set; }
        public string? status { get; set; }

		[NotMapped]
        public double TotalPrice { get; set; }
    }
}

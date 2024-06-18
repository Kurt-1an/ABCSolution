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
	public class Product
	{
		[Key]
		public int Id { get; set; }

		public DateTime StartSalesDate { get; set; } = DateTime.UtcNow;
		public DateTime? EndSalesDate { get; set; }
		//Product Image
		[DisplayName("For Inventory")]
		[ValidateNever]
		public string? ImageUrl { get; set; }
		public long Barcode { get; set; }
		public string SKU { get; set; }
        public string? status { get; set; }

        [Required]
		[DisplayName("Product Name")]
		public string productName { get; set; }
		[DisplayName("Category")]
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		[ValidateNever]
		public Category Category { get; set; }
		public string Brand { get; set; }

		// public int StockPerStoreId { get; set; }
		public StockPerStore StockPerStore { get; set; }

        [Required]
        public string? Description { get; set; }
		
		[DisplayName("Cost Price")]
		public float CostPrice { get; set; }
		[Required]
		[DisplayName("Retail Price")]
		public float RetailPrice { get; set; }
		
		//Warranty
		[Required]
		public string WarrantyType { get; set; }
		[Required]
		public string Duration { get; set; }
		
		//Foreign Key Relation of Supplier
		[DisplayName("Supplier")]
		public int SupplierId { get; set; }
		[ForeignKey("SupplierId")]
		[ValidateNever]
		public Supplier Supplier { get; set; }
		public bool? IsRemoved { get; set; } = false;


		[NotMapped]
		public int Quantity { get; set; }
	}
}


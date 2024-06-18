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
	public class ShoppingCart
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string ProductName { get; set; } // Add ProductName property

		
		[ForeignKey("ProductId")]
		public Product Product { get; set; }

		[Range(1, 100, ErrorMessage = "Please Enter a value between 1 and 20")]
		public int Quantity { get; set; }

		public string? ApplicationUserId { get; set; }
		[ForeignKey("ApplicationUserId")]
		public ApplicationUser ApplicationUser { get; set; }

		[NotMapped]
		public double Price { get; set; }
	}
}

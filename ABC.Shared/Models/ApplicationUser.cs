using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABC.Shared.Models
{
    public class ApplicationUser: IdentityUser
    {
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }

		[Display(Name = "Store Name")]
		public string? StoreName { get; set; }

		public int? StoreId { get; set; }
		[ForeignKey("StoreId")]
		[ValidateNever]
		public Store? Store { get; set; }

		[NotMapped]
        public string Role { get; set; }
    }
}

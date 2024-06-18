using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ABC.Shared.Models
{
    public class Content
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("About")]
        public string? About { get; set; }

		[DisplayName("Privacy")]
		public string? Privacy { get; set; }

		[DisplayName("Vission & Mission")]
		public string? VissionMission { get; set; }


		[DisplayName("Terms & Policy")]
        public string? TermsPolicy { get; set; }


        [DisplayName("Return & Refund Process")]
        public string? returnRefund { get; set; }


    }
}

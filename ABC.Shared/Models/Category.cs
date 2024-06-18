using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ABC.Shared.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(30)]
        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        public bool? status { get; set; }

    }
}

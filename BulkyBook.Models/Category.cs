using System.ComponentModel.DataAnnotations;

namespace BulkiBook.Models
{
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;
        [Range(0,100,ErrorMessage = "Range must be between 0 to 100 !!")]
        [Display(Name ="Display Order")]
        //[ValidateNever]
        public int DisplayOrder { get; set; }
    }
}

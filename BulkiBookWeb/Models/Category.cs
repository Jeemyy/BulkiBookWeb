using System.ComponentModel.DataAnnotations;

namespace BulkiBookWeb.Models
{
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Range(0,100,ErrorMessage = "Range must be between 0 to 100 !!")]
        public int DisplayOrder { get; set; }
    }
}

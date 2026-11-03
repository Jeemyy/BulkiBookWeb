using BulkiBook.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models
{
    public class ShoppingCart : BaseEntity
    {
        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 to 1000")]
        public int Count { get; set; }
        [NotMapped]
        public double Price
        {
            get
            {
                if (Product == null) return 0;
                if (Count < 50) { return Product.Price; }
                else if (Count < 100) { return Product.Price50; }
                else { return Product.Price100; }
            }
            set;
        }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}

using BulkiBook.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models
{
    public class OrderHeader : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public DateTime OrderShipping { get; set; }
        public double OrderTotal { get; set; }
        public string? OrderStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; } 

        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string StreetAddress{ get; set; }
        [Required]
        public string PostalCode { get; set; }

        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}

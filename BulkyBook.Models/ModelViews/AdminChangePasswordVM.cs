using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models.ModelViews
{
    public class AdminChangePasswordVM
    {
        public string UserId { get; set; }
        [Display(Name = "User Email")]
        public string UserEmail { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The Password Confirmation Not Match!")]
        public string ConfirmPassword { get; set; }
    }
}

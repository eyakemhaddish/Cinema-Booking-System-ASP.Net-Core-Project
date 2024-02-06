using System.ComponentModel.DataAnnotations;

namespace FromScratch_.net_5.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string Currentpassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage ="The new Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}

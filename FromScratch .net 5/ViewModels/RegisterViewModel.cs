using System.ComponentModel.DataAnnotations;

namespace FromScratch_.net_5.ViewModels
{
    public class RegisterViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        //Compare password field and confirm password
        [Compare("Password",
            ErrorMessage="Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string City { get; set; }

    }
}

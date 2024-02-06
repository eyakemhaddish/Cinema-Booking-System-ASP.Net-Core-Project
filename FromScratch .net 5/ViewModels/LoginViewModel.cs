using Microsoft.AspNetCore.Authentication;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FromScratch_.net_5.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

               [Display(Name = "Confirm Password")]
               public bool RememberMe { get; set; } 

        public string ReturnUrl { get; set; }   
        
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}

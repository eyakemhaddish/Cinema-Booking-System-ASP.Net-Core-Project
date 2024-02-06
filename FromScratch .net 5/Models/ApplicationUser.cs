using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FromScratch_.net_5.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
     
        public bool IsCustomMethod()
        {
            return true;
        }

    }
}

using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FromScratch_.net_5.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>(); 
            Roles = new List<string>();
        }

        public string Id { get; set; }


        public string UserName { get; set;}
        [Required]
        [EmailAddress]
        public string Email { get; set; }

       
       
        public string City { get; set; }

        public List<string> Roles { get; set;}

        public IList<string> Claims { get; set;}

        //public List<string> Users { get; set; }
    }
}

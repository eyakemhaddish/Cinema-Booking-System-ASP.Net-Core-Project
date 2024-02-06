using System.ComponentModel.DataAnnotations;

namespace FromScratch_.net_5.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}

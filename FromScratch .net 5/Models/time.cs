using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FromScratch_.net_5.Models
{
    public enum time
    {
        None,
        [Display(Name = "6:00 PM")]
        One,
        [Display(Name = "7:30 PM")]
        Two,
        [Display(Name = "8:30 PM")]
        Three,
        [Display(Name = "9:30 PM")]
        Four,
        [Display(Name = "11:00 PM")]
        Five,
        [Display(Name = "12:00 AM")]
        Six,
        [Display(Name = "2:00 AM")]
        Seven
        
    }
}

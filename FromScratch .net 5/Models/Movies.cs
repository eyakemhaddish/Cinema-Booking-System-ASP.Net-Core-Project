using FromScratch_.net_5.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagement.Models
{
    public class Movies
    {
        
        public int Id { get; set; }
        [Required]
        [MaxLength(100,ErrorMessage ="Name cannot exceed 100 characters")]
        public string Name{ get; set; }

      
        public string Description { get; set; }
        [Required]
        public genre? Genre { get; set; }
        [Required]
        //[RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        public string Director { get; set; }

        public string Actors { get; set; }

        public string Producer { get; set; }

       
        public DateTime? ReleaseDate { get; set; }

        public DateTime? AvailableUntil { get; set; }
        public int? AvailableSeats { get; set; }
        public time? AvailableTimes { get; set; }
        //public string Producer { get; set; }

        public string Photo { get; set; }

    }
}

using FromScratch_.net_5.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FromScratch_.net_5.ViewModels
{
    //why
 
    //We have to create a navigation property wwhich will complicate
    //things that is why we use another view models 
    public class MovieCreateViewModel
    {
        
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }
        [Required]
        public genre? Genre { get; set; }

        public string Description { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        public string Director { get; set; }

        public string Producer { get; set; }
        public string Actors { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public DateTime? AvailableUntil { get; set; }
        public int? AvailableSeats { get; set; }
        public time? AvailableTimes { get; set; }
        public IFormFile Photo { get; set; }
    }
}
 
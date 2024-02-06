using FromScratch_.net_5.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FromScratch_.net_5.Models
{
    public class paid
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }
        public string UserId { get; set; }
        public string MovieId { get; set; }
        public string children { get; set; }
        public string adults { get; set; }
        public string price { get; set; }
        public string email { get; set; }
        public string MovieName { get; set; }
        public DateTime BookingDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string Status { get; set; }
    }
}

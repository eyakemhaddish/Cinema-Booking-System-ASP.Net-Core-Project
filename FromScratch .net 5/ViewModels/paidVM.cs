using System;
using System.ComponentModel.DataAnnotations;

namespace FromScratch_.net_5.ViewModels
{
    public class paidVM
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string MovieId { get; set; }
        public string children { get; set; }
        public string adults { get; set; }
        public string price { get; set; }
        public string email { get; set; }

        public string transaction_num { get; set; }

        public static implicit operator paidVM(Models.paid v)
        {
            throw new NotImplementedException();
        }
    }
}


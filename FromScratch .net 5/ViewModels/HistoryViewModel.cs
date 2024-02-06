using Microsoft.VisualBasic;
using MovieManagement.Models;
using System;

namespace FromScratch_.net_5.ViewModels
{
    public class HistoryViewModel
    {
        public string UserId { get; set; }
        public string MovieId { get; set; }
        public string MovieName { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string Code { get; set; }
        public string Children { get; set; }
        public string Adults { get; set; }
        public string Status { get; set; }
        public string price { get; set; }
            public string PageTitle { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }
        public string photopath { get; set; }

       
    }

}


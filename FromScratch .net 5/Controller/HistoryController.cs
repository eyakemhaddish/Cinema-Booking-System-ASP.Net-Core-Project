using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using FromScratch_.net_5.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using FromScratch_.net_5.ViewModels;
using System.Linq;
using MovieManagement.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FromScratch_.net_5.Controller3455
{
    public class HistoryController  : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly Models.paid pay;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AppDbContext appDbContext;

        public HistoryController(IMovieRepository employeeRepository,Models.paid pay, SignInManager<ApplicationUser> signInManager, AppDbContext appDbContext)
        {
            _movieRepository = employeeRepository;
            this.pay = pay;
            this.signInManager = signInManager;
            this.appDbContext = appDbContext;
        }


        [Authorize]
        public async Task<ActionResult> History()
        {
            var userId = signInManager.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pays = await appDbContext.paid
                .OrderByDescending(p => p.Id)
                .ToListAsync();
            var movies = await appDbContext.Movies.ToListAsync();

            var History = new List<HistoryViewModel>();
            foreach (var pay in pays)
            {
                if(signInManager.IsSignedIn(User) && User.IsInRole("Super Admin"))
                    {
                    Movies movie = _movieRepository.GetMovies(int.Parse(pay.MovieId));
                    HistoryViewModel historyViewModel = new HistoryViewModel
                    {
                        UserId = pay.UserId,
                        MovieId = pay.MovieId,
                        Children = pay.children,
                        Adults = pay.adults,
                        MovieName = pay.MovieName,
                        price = pay.price,
                        Status = pay.Status,
                        Email = pay.email,
                        ExpiryDate = pay.ExpiryDate,
                        Code = pay.Code,
                        Id = pay.Id,
                        photopath = movie.Photo
                        // Add more properties as needed
                    };

                    History.Add(historyViewModel);
                }
                else
                {
                    if (userId == pay.UserId)
                    {
                        // If the signed-in user ID matches the user ID from the SQL table,
                        // return the view with the Pay model data
                        Movies movie = _movieRepository.GetMovies(int.Parse(pay.MovieId));
                        HistoryViewModel historyViewModel = new HistoryViewModel
                        {
                            UserId = pay.UserId,
                            MovieId = pay.MovieId,
                            Children = pay.children,
                            Adults = pay.adults,
                            MovieName = pay.MovieName,
                            price = pay.price,
                            Status = pay.Status,
                            Email = pay.email,
                            ExpiryDate = pay.ExpiryDate,
                            Code = pay.Code,
                            Id = pay.Id,
                            photopath = movie.Photo
                            // Add more properties as needed
                        };

                        History.Add(historyViewModel);
                    }
                }
                    
            }
          
                // If there are payment details for the matching user ID, return the view with the payment details
                return View(History);
            
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> DeleteHistory(int Id)
        {
            if (Id == 0)
            {

            }
            var pays = await appDbContext.paid.FindAsync(Id);

            if (pays != null)
            {
                appDbContext.paid.Remove(pays);
                appDbContext.SaveChanges();

            }
            
            return RedirectToAction("History");
        }

        [HttpGet]
        public async Task<ActionResult> Stat()
        {
            // Get the start and end dates for the current week
            var today = DateTime.Now;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            // Create an array to store the counts for each day
            var bookingsPerDay = new int[7];

            // Loop through each day of the week and count the number of bookings for that day
            for (DateTime date = startOfWeek; date < endOfWeek; date = date.AddDays(1))
            {
                var dayOfWeek = (int)date.DayOfWeek;
                var pays = await appDbContext.paid
                    .Where(p => p.BookingDate.Date == date.Date)
                     .Where(p => p.Status == "Success")
                    .ToListAsync();
                int todayCount = pays.Count;
                bookingsPerDay[dayOfWeek] = todayCount;
            }

            ViewBag.Monday = bookingsPerDay[1];
            ViewBag.Tuesday = bookingsPerDay[2];
            ViewBag.Wednesday = bookingsPerDay[3];
            ViewBag.Thursday = bookingsPerDay[4];
            ViewBag.Friday = bookingsPerDay[5];
            ViewBag.Saturday = bookingsPerDay[6];
            ViewBag.Sunday = bookingsPerDay[0];





            // Get the start and end dates for the current month
          
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            // Retrieve the top 5 most booked movies for the current week
            var query = appDbContext.Set<paid>()
                .Where(p => p.BookingDate >= startOfMonth && p.BookingDate <= endOfMonth)
                .Where(p => p.Status == "Success")
                .GroupBy(p => p.MovieName)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => new { MovieName = g.Key, BookingCount = g.Count() })
                .ToList();

            // Store the movie names and counts in separate arrays
            string[] movieNames = query.Select(q => q.MovieName).ToArray();
            int[] counts = query.Select(q => q.BookingCount).ToArray();
            // Calculate the total bookings for the top 5 movies
            int total = counts.Take(5).Sum();

            
            ViewBag.TotalBooking = total;
            ViewBag.Movie1 = movieNames[0];
                ViewBag.count1 = counts[0];
            
           
            if (movieNames.Length > 1)
            {
                ViewBag.Movie2 = movieNames[1];
                ViewBag.count2 = counts[1];
            }
               
            if (movieNames.Length > 2)
            {
                ViewBag.Movie3 = movieNames[2];
                ViewBag.count3 = counts[2];
            }
              if(movieNames.Length > 3)
            {
                ViewBag.Movie4 = movieNames[3];
                ViewBag.count4 = counts[3];
            }
           if(movieNames.Length > 4)
            {
                ViewBag.Movie5 = movieNames[4];
                ViewBag.count5 = counts[4];
            }
          

            var revenue = appDbContext.paid
    .Where(p => p.BookingDate >= startOfMonth && p.BookingDate <= endOfMonth)
    .Where(p => p.Status == "Success")
    .AsEnumerable()
    .Select(p => int.Parse(p.price))
    .Sum();

            ViewBag.Totalrevenue = revenue;

            var kids = appDbContext.paid
     .Where(p => p.BookingDate >= startOfMonth && p.BookingDate <= endOfMonth)
     .Where(p => p.Status == "Success")
      .AsEnumerable()
     .GroupBy(p => p.MovieId)
     .Select(g => new
     {
         MovieId = g.Key,
         TotalChildren = g.Sum(p => int.Parse(p.children))
     })
     .OrderByDescending(g => g.TotalChildren)
     .FirstOrDefault();



            Movies childrenfavourite = _movieRepository.GetMovies(int.Parse(kids.MovieId));

            ViewBag.Photo = childrenfavourite.Photo;
            ViewBag.KidsName = childrenfavourite.Name;

            var TopUsers = appDbContext.Set<paid>()
               .Where(p => p.BookingDate >= startOfMonth && p.BookingDate <= endOfMonth)
               .Where(p => p.Status == "Success")
                .AsEnumerable()
               .GroupBy(p => p.email)
               .OrderByDescending(g => g.Count())
               .Take(3)
               .Select(g => new {
                   User = g.Key,
                   BookingCount = g.Count(),
                   TotalPrice = g.Sum(p => int.Parse(p.price))
               })
               .ToList();

            string[] topUsers = TopUsers.Select(q => q.User).ToArray();
            int[] BookingCount = TopUsers.Select(q => q.BookingCount).ToArray();
            int[] TotalPrice = TopUsers.Select(q => q.TotalPrice).ToArray();

            ViewBag.user1 = topUsers[0];
            ViewBag.topcount1 = BookingCount[0];
            ViewBag.price1 = TotalPrice[0];

            if (topUsers.Length > 1)
            {
                ViewBag.user2 = topUsers[1];
                ViewBag.topcount2 = BookingCount[1];
                ViewBag.price2 = TotalPrice[1];
            }
            if (topUsers.Length > 2)
            {
                ViewBag.user3 = topUsers[2];
                ViewBag.topcount3 = BookingCount[2];
                ViewBag.price3 = TotalPrice[2];
            }

            return View();
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Sort(string status)
        {
           

            var userId = signInManager.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pays = await appDbContext.paid.OrderByDescending(p => p.Id).ToListAsync();
            var movies = await appDbContext.Movies.ToListAsync();
            var History = new List<HistoryViewModel>();

            if (User.IsInRole("Super Admin")) {
                if (status == null)
                {
                    foreach (var pay in pays)
                    {
                        Movies movie = _movieRepository.GetMovies(int.Parse(pay.MovieId));
                        HistoryViewModel historyViewModel = new HistoryViewModel
                        {
                            UserId = pay.UserId,
                            MovieId = pay.MovieId,
                            Children = pay.children,
                            Adults = pay.adults,
                            MovieName = pay.MovieName,
                            price = pay.price,
                            Status = pay.Status,
                            Email = pay.email,
                            ExpiryDate = pay.ExpiryDate,
                            Code = pay.Code,
                            Id = pay.Id,
                            photopath = movie.Photo
                            // Add more properties as needed
                        };
                        History.Add(historyViewModel);
                    }

                }

                foreach (var pay in pays)
                {
                    if (pay.Status == status || pay.MovieName == status) // to filter by status
                    {
                        Movies movie = _movieRepository.GetMovies(int.Parse(pay.MovieId));
                        HistoryViewModel historyViewModel = new HistoryViewModel
                        {
                            UserId = pay.UserId,
                            MovieId = pay.MovieId,
                            Children = pay.children,
                            Adults = pay.adults,
                            MovieName = pay.MovieName,
                            price = pay.price,
                            Status = pay.Status,
                            Email = pay.email,
                            ExpiryDate = pay.ExpiryDate,
                            Code = pay.Code,
                            Id = pay.Id,
                            photopath = movie.Photo
                            // Add more properties as needed
                        };

                        History.Add(historyViewModel);
                    }
                }
            }
            else
            {
                if (status == null)
                {
                    foreach (var pay in pays)
                    {
                        if (userId == pay.UserId)
                        {
                            Movies movie = _movieRepository.GetMovies(int.Parse(pay.MovieId));
                            HistoryViewModel historyViewModel = new HistoryViewModel
                            {
                                UserId = pay.UserId,
                                MovieId = pay.MovieId,
                                Children = pay.children,
                                Adults = pay.adults,
                                MovieName = pay.MovieName,
                                price = pay.price,
                                Status = pay.Status,
                                Email = pay.email,
                                ExpiryDate = pay.ExpiryDate,
                                Code = pay.Code,
                                Id = pay.Id,
                                photopath = movie.Photo
                                // Add more properties as needed
                            };
                            History.Add(historyViewModel);
                        }
                            
                    }

                }

                foreach (var pay in pays)
                {
                    if (pay.Status == status && userId == pay.UserId || pay.MovieName == status && userId == pay.UserId) // to filter by status
                    {
                        Movies movie = _movieRepository.GetMovies(int.Parse(pay.MovieId));
                        HistoryViewModel historyViewModel = new HistoryViewModel
                        {
                            UserId = pay.UserId,
                            MovieId = pay.MovieId,
                            Children = pay.children,
                            Adults = pay.adults,
                            MovieName = pay.MovieName,
                            price = pay.price,
                            Status = pay.Status,
                            Email = pay.email,
                            ExpiryDate = pay.ExpiryDate,
                            Code = pay.Code,
                            Id = pay.Id,
                            photopath = movie.Photo
                            // Add more properties as needed
                        };

                        History.Add(historyViewModel);
                    }
                }
            }
           

            // If there are payment details for the matching user ID, return the view with the payment details
            return View("History",History);
        }

    }

     
    }


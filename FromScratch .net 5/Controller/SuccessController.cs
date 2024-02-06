using FromScratch_.net_5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using MovieManagement.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using static Microsoft.ClearScript.V8.V8CpuProfile;

namespace FromScratch_.net_5.Controller5
{
    public class SuccessController : Controller
    {
        
        private readonly AppDbContext context;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMovieRepository movieRepository;

        public SuccessController(AppDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMovieRepository movieRepository)
        {
            this.context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.movieRepository = movieRepository;
        }

        // GET: SuccessController
        [HttpPost]
        public async Task<IActionResult> SaveUserData(string amount, string ppl, string children, string email,string movieId, int userId, string code )
        {
            var user = await userManager.GetUserAsync(User);
            var userIdd =user.Id ;
            var ExpiryDate = DateTime.Now.AddDays(7);
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
           
            TempData["price"] = amount;
            TempData["ppl"] = ppl;
            TempData["email"] = email;
            TempData["children"]= children;
            TempData["movieId"] = movieId;
            TempData["Expiry"] = ExpiryDate.ToString();
         
            if(code==null)
            {
                code = new string(
                  Enumerable.Repeat(chars, 8)
                            .Select(s => s[random.Next(s.Length)])
                            .ToArray());
              
                Movies movie = movieRepository.GetMovies(int.Parse(movieId));
                paid newPayer = new paid
                {
                    UserId = userIdd,
                    MovieId = movieId,
                    children = children,
                    adults = ppl,
                    price = amount,
                    email = email,
                    Code = code,
                    MovieName = movie.Name,
                    BookingDate = DateTime.Now,
                    ExpiryDate = ExpiryDate,
                    Status = "Fail"
                };

                context.paid.Add(newPayer);
                context.SaveChanges();

            }
            TempData["code"] = code;


            return RedirectToAction("ToChapa", "Success");
           
        }

        public IActionResult ToChapa()
        {
            string price = TempData["price"] as string;
            TempData.Keep("price");
            string email = TempData["email"] as string;
            string people = TempData["ppl"] as string;
            TempData.Keep("ppl");
            TempData.Keep("email");
            


          

            ViewBag.Price = price;
            ViewBag.email = email;
            return View();

        }
        public IActionResult Success()
        {
            string code = TempData["code"] as string;
            TempData.Keep("code");
            var row = context.paid.FirstOrDefault(r => r.Code == code);

            if (row != null)
            {
                // Update the column value
                row.Status = "Success";

                // Save the changes to the database
                context.SaveChanges();
            }
            string movieId = TempData["movieId"] as string ;

            var row2 = context.Movies.FirstOrDefault(r => r.Id == int.Parse(movieId));
            if (row2 != null)
            {
                // Update the column value
                row2.AvailableSeats = row2.AvailableSeats-1;

                // Save the changes to the database
                context.SaveChanges();
            }


            string price = TempData["price"] as string;
            TempData.Keep("price");
            string people = TempData["ppl"] as string;
            TempData.Keep("ppl");
            string email = TempData["email"] as string;
            TempData.Keep("email");
            string children = TempData["children"] as string;
            TempData.Keep("children");
            string ExpiryDate = TempData["Expiry"] as string;
            Movies movie = movieRepository.GetMovies(int.Parse(movieId));
            ViewBag.movie = movie.Name;
            ViewBag.Price = price;
            ViewBag.adults = people;
            ViewBag.email = email;
            ViewBag.Code = TempData["code"] as string; 
            ViewBag.children = children;
            ViewBag.ExpiryDate = ExpiryDate;
            return View();

        }

        //To display Qr code from Booking History
        public IActionResult GetQr(string email, string adults, string children, string Expiry, string price, string code, string movieName)
        {
            ViewBag.movie = movieName;
            ViewBag.Price = price;
            ViewBag.adults = adults;
            ViewBag.email = email;
            ViewBag.Code = code;
            ViewBag.children = children;
            ViewBag.ExpiryDate = Expiry;
            return View();
        }

    }
}

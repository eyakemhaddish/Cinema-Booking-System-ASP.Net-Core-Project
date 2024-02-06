using FromScratch_.net_5.Models;
using FromScratch_.net_5.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieManagement.Models;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace FromScratch_.net_5.Controller2
{
    public class BookController : Controller
    {

        private readonly IMovieRepository _movieRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext dbcontext;

        public BookController(IMovieRepository employeeRepository, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, AppDbContext dbcontext)
        {
            _movieRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.userManager = userManager;
            this.dbcontext = dbcontext;
        }



        [HttpGet]
        [Authorize]
        public ViewResult Book(int id)
        {

            Movies movie = _movieRepository.GetMovies(id);
            TempData["movieId"] = id.ToString();
            if (movie == null)
            {
                //ViewBag.Id = id;
                Response.StatusCode = 404;
                return View("EmployeeNotFound");

            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Movie = movie,
                PageTitle = "Movie Details"
                // Employee model =_employeeRepository.GetEmployee(1);
            };

            return View(homeDetailsViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Book(string price, string numberOfPeople, string children )
        {
            //var price = data["price"].ToString();
            var user = await userManager.GetUserAsync(User);

            var userEmail = user.Email;
            string movieId = TempData["movieId"] as string;
            ViewBag.Price = price;
            ViewBag.Children = children;
            ViewBag.movieId = movieId;
            ViewBag.Adults = numberOfPeople;
          
            ViewBag.email = userEmail;
            return View("checkout");


        }
      
    }
}

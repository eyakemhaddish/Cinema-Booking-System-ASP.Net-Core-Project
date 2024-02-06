using MovieManagement.Models;
using FromScratch_.net_5.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Identity;
using FromScratch_.net_5.Models;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;

namespace FromScratch_.net_5
{
    //   [Route("[controller]/[action]")]
    [Authorize(Roles = "Admin, Super Admin")] 
    public class HomeController : Controller 

        
    {

        private readonly IMovieRepository _movieRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IStringLocalizer<SharedResources> _localizer;
   
        private readonly AppDbContext context;

        public HomeController(IMovieRepository employeeRepository, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, IStringLocalizer<SharedResources> localizer,AppDbContext _context)
        {
            _movieRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.userManager = userManager;
            _localizer = localizer;
           
            context = _context;
        }



        //Action methods within controller to handle http request
        // [Route("~/")]
        // [Route("~/Home")]

     

        [AllowAnonymous]
        public ViewResult Home()
        {
            ViewData["Welcome"] = _localizer["Welcome"];
            return View();



        }
        [AllowAnonymous]
        public ViewResult Index() {
        
            var model =  _movieRepository.GetAllMovie();
            return View(model);


        
        }
        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            //ViewData["Employee"] = model; 
            //ViewData["PageTitle"] = "Employee Details";

            //ViewBag.Employee = model;
            //ViewBag.PageTitle = "Employee Details";

            //ViewBag.PageTitle = "Employee Details";
            Movies movie = _movieRepository.GetMovies(id);

            if(movie == null)
            {
                ViewBag.Id = id;
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

        [HttpGet]
        [Authorize(Policy = "CreateMoviePolicy")]
        public ViewResult Create() {
            return View();
        }

      
        [HttpGet]
        [Authorize(Policy = "EditMoviePolicy")]
        public ViewResult Edit( int id)
        {       
            Movies movie = _movieRepository.GetMovies(id);
            //EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            //{
            //    Id = employee.Id,
            //    Name = employee.Name,
            //    Email = employee.Email,
            //    Department = employee.Department,
            //    ExistingPhotoPath = employee.Photo

            //};   
            ViewBag.Id = movie.Id;
            ViewBag.Name = movie.Name;
            ViewBag.Email = movie.Director;
            ViewBag.Description = movie.Description;
            ViewBag.Department = movie.Genre;
            ViewBag.Producer = movie.Producer;
            ViewBag.Actors = movie.Actors;
            ViewBag.AvailableSeats = movie.AvailableSeats;

            ViewBag.ExistingPhotoPath = movie.Photo;
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "EditMoviePolicy")]
        public IActionResult Edit(MovieEditViewModel model)
        {
       //     var Employee = _employeeRepository.GetEmployee(id);

            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                Movies movie = _movieRepository.GetMovies(model.Id);
                movie.Name = model.Name;
                movie.Director = model.Director;
                movie.Producer = model.Producer;
                movie.ReleaseDate = model.ReleaseDate;
                movie.Description = model.Description;
                movie.Actors = model.Actors;
                movie.AvailableSeats = model.AvailableSeats;
                movie.Genre = model.Genre;
                if (model.Photo != null)
                {
                    //Check if there employee has an existing image and delete if it does
                    if(model.ExistingPhotoPath != null)
                    {
                       string filePath =  Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }

                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                    // string filepath = Path.Combine(uploadFolder, uniqueFileName);
                    //model.Photo.CopyTo(new FileStream(filepath, FileMode.Create));

                    using (var stream = new FileStream(Path.Combine(uploadFolder, uniqueFileName), FileMode.Create))
                    {
                        model.Photo.CopyTo(stream);
                    }
                movie.Photo = uniqueFileName;
                }
               
            _movieRepository.Update(movie);
                return RedirectToAction("Index");
            }
            return View();
        }



        [HttpPost]
        [Authorize(Policy = "CreateMoviePolicy")]
        public IActionResult Create(MovieCreateViewModel model)
        {
            
            if (ModelState.IsValid) {
                string uniqueFileName = null;
                if(model.Photo != null) {
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                    // string filepath = Path.Combine(uploadFolder, uniqueFileName);
                    //model.Photo.CopyTo(new FileStream(filepath, FileMode.Create));
                
                    using (var stream = new FileStream(Path.Combine(uploadFolder, uniqueFileName), FileMode.Create))
                    {
                        model.Photo.CopyTo(stream);
                    }
                }
                Movies newMovie = new Movies
                {
                    Name = model.Name,
                    Director = model.Director,
                    Genre = model.Genre,
                    Actors = model.Actors,
                    Producer = model.Producer,
                    Description = model.Description,    
                    ReleaseDate = model.ReleaseDate,
                    AvailableUntil = model.AvailableUntil,
                    AvailableSeats = model.AvailableSeats,
                    Photo = uniqueFileName

                };

                _movieRepository.Add(newMovie);
         //   return RedirectToAction("index", "home");
            return RedirectToAction("details", new { id = newMovie.Id });
            }

            return View();
        }

        [Authorize(Policy = "DeleteMoviePolicy")]
        public ActionResult Delete(int id) {
            _movieRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}  
 
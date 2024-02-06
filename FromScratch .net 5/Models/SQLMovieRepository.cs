using MovieManagement.Models;
using System.Collections.Generic;

namespace FromScratch_.net_5.Models
{
    public class SQLMovieRepository : IMovieRepository
    {
        private readonly AppDbContext context;

        public SQLMovieRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Movies Add(Movies movies)
        {
            context.Movies.Add(movies);
            context.SaveChanges();
            return movies;
        }

        public Movies Delete(int Id)
        {
            Movies movie = context.Movies.Find(Id);
            if(movie != null) { 
            context.Movies.Remove(movie);
                context.SaveChanges();
            }
            return movie;    
        }

        public IEnumerable<Movies> GetAllMovie()
        {
           return context.Movies;
        }

        public Movies GetMovies(int Id)
        {
            return context.Movies.Find(Id);
        }

        public Movies Update(Movies employeeChanges)
        {
            var employee = context.Movies.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeeChanges;
        }
    }
}

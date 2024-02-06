 using FromScratch_.net_5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagement.Models
{
    public class MockMovieRepository  : IMovieRepository
    {
        public List<Movies> _movieList;

        public MockMovieRepository() {
            _movieList = new List<Movies>(){
   new Movies() { Id = 1, Name = "Sunny", Genre = genre.Comedy,Director ="Sunny@gmail.com" },
   new Movies() { Id=2, Name="Pinki", Genre = genre.Action,Director ="Poinki@gmail.com"},
   new Movies() { Id=3, Name="Tensy", Genre =genre.Fiction,Director ="Tensy@gmail.com"},
   new Movies() { Id=4, Name="Bobby", Genre =genre.Horror,Director ="Bobby@gmail.com"},
   new Movies() { Id=5, Name="Sweety", Genre =genre.Action,Director ="Sweety@gmail.com"}
};
        }

        public Movies Add(Movies movie)
        {

            movie.Id = _movieList.Max(e => e.Id) + 1;

            _movieList.Add(movie);
            return movie;
        }

        public Movies Delete(int Id)
        {
            Movies movie= _movieList.FirstOrDefault(e => e.Id == Id);
            if (movie != null)
            {
                _movieList.Remove(movie);
            }
            return movie;
        }

        public IEnumerable<Movies> GetAllMovie()
        {
            return _movieList;
        }

        public Movies GetMovies(int Id) 
        {
            return _movieList.FirstOrDefault(e => e.Id ==Id);  
        }

        public Movies Update(Movies movieChanges)
        {
            Movies movie = _movieList.FirstOrDefault(e => e.Id == movieChanges.Id);
            if (movie != null)
            {
               movie.Name = movieChanges.Name;
                movie.Director = movieChanges.Director;

                movie.Genre = movieChanges.Genre;

            }
            return movie;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagement.Models
{
    public interface IMovieRepository
    {
        Movies GetMovies(int Id); 
    IEnumerable<Movies> GetAllMovie();
        Movies Add(Movies employee);    
        Movies Update(Movies employeeChanges);
        Movies Delete(int Id);
    }
}

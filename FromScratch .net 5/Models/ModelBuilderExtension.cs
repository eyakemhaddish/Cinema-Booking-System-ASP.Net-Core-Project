using MovieManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace FromScratch_.net_5.Models
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movies>().HasData(
                          new Movies
                          {
                              Id = 3,
                              Name = "Mike",
                              Genre = genre.Fiction,
                              Director = "Mike@kirswaksi.com"


                          },
                          new Movies
                          {
                              Id = 2,
                              Name = "John",
                              Genre = genre.Comedy,
                              Director = "John@kirswaksi.com"


                          }

                           );

        }
    }
}

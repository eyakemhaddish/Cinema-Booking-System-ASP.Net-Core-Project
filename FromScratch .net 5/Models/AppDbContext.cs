using MovieManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;

namespace FromScratch_.net_5.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<paid> paid { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        
       {
         
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();    
        }
    }

}

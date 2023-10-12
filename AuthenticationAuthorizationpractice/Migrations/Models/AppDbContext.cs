using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthenticationAuthorizationpractice.Models;
using Microsoft.Extensions.Hosting;

namespace AuthenticationAuthorizationpractice.Models
{
    public class AppDbContext: IdentityDbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }



        public DbSet<Employee> TblEmployee { get; set; }
        public DbSet<Designation> TblDesignation { get; set; }

       

        protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
            }

        }
}

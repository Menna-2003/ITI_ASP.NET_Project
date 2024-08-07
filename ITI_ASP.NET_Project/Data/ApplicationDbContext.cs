using ITI_ASP.NET_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ITI_ASP.NET_Project.Data {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext ( DbContextOptions<ApplicationDbContext> options ) : base( options ) {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Person> person { get; set; }
		public DbSet<Cart> cart { get; set; }

    }
}

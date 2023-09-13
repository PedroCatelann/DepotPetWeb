using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { 
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> User { get; set; }
    }
}

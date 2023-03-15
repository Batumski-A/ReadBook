using Microsoft.EntityFrameworkCore;
using ReadBook.Models.Pdf;

namespace ReadBook.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }
        public DbSet<AllBook> allBooks { get; set; }
        public DbSet<Content> contents { get; set; }
    }
}

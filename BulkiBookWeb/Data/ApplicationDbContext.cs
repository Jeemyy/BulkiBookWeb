using BulkiBookWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkiBookWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
        DbSet<Category> Categories { get; set; }
    }
}

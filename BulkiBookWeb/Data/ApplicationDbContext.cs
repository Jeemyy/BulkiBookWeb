using BulkiBookWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkiBookWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Category>()
                .HasData(
                    new Category { 
                        Id = 1,
                        Name = "Action",
                        CreatedAt = new DateTime(2026, 9, 22, 0, 0, 0, DateTimeKind.Utc),
                        DisplayOrder = 1
                    },
                    new Category {
                        Id = 2,
                        Name = "Faylasofi",
                        CreatedAt = new DateTime(2026, 9, 22, 0, 0, 0, DateTimeKind.Utc),
                        DisplayOrder = 1
                    },
                    new Category {
                        Id = 3,
                        Name = "History",
                        CreatedAt = new DateTime(2026, 9, 22, 0, 0, 0, DateTimeKind.Utc),
                        DisplayOrder = 1
                    }
                ); 
            base.OnModelCreating(modelBuilder);
        }
    }
}

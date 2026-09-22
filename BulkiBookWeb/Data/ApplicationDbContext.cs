using BulkiBookWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;

namespace BulkiBookWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
        DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Category>()
                .HasData(
                    new Category { 
                        Id = 1,
                        Name = "Action",
                        CreatedAt = new DateTime(2026, 9, 22, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Category { Id = 2, Name = "Faylasofi", CreatedAt = new DateTime(2026, 9, 22, 0, 0, 0, DateTimeKind.Utc) },
                    new Category { Id = 3, Name = "History", CreatedAt = new DateTime(2026, 9, 22, 0, 0, 0, DateTimeKind.Utc) }
                ); 
            base.OnModelCreating(modelBuilder);
        }
    }
}

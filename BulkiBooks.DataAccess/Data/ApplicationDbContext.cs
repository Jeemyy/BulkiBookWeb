using BulkiBook.Models;
using BulkyBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulkiBook.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    CreatedAt = new DateTime(2026, 1, 1),
                    Title = "The Silent Patient",
                    Description = "A psychological thriller about a famous painter who suddenly stops speaking.",
                    ISBN = "9781250301697",
                    Author = "Alex Michaelides",
                    ListPrice = 25.00,
                    Price = 23.00,
                    Price50 = 21.00,
                    Price100 = 19.00,
                    CategoryId = 1,
                    ImageUrl = ""
                },

                new Product
                {
                    Id = 2,
                    CreatedAt = new DateTime(2026, 1, 2),
                    Title = "Atomic Habits",
                    Description = "A practical guide to building good habits and breaking bad ones.",
                    ISBN = "9780735211292",
                    Author = "James Clear",
                    ListPrice = 30.00,
                    Price = 28.00,
                    Price50 = 26.00,
                    Price100 = 24.00,
                    CategoryId = 2,
                    ImageUrl = ""
                },

                new Product
                {
                    Id = 3,
                    CreatedAt = new DateTime(2026, 1, 3),
                    Title = "Clean Code",
                    Description = "A handbook of agile software craftsmanship and writing maintainable code.",
                    ISBN = "9780132350884",
                    Author = "Robert C. Martin",
                    ListPrice = 45.00,
                    Price = 42.00,
                    Price50 = 39.00,
                    Price100 = 36.00,
                    CategoryId = 3,
                    ImageUrl = ""
                },

                new Product
                {
                    Id = 4,
                    CreatedAt = new DateTime(2026, 1, 4),
                    Title = "The Pragmatic Programmer",
                    Description = "Practical advice and techniques for becoming a better software developer.",
                    ISBN = "9780135957059",
                    Author = "David Thomas",
                    ListPrice = 40.00,
                    Price = 37.00,
                    Price50 = 34.00,
                    Price100 = 31.00,
                    CategoryId = 3,
                    ImageUrl = ""
                },

                new Product
                {
                    Id = 5,
                    CreatedAt = new DateTime(2026, 1, 5),
                    Title = "Rich Dad Poor Dad",
                    Description = "A personal finance book about financial education and investing.",
                    ISBN = "9781612681139",
                    Author = "Robert T. Kiyosaki",
                    ListPrice = 22.00,
                    Price = 20.00,
                    Price50 = 18.00,
                    Price100 = 16.00,
                    CategoryId = 2,
                    ImageUrl = ""
                },

                new Product
                {
                    Id = 6,
                    CreatedAt = new DateTime(2026, 1, 6),
                    Title = "Harry Potter and the Sorcerer's Stone",
                    Description = "The first adventure of Harry Potter at Hogwarts School of Witchcraft and Wizardry.",
                    ISBN = "9780590353427",
                    Author = "J.K. Rowling",
                    ListPrice = 28.00,
                    Price = 26.00,
                    Price50 = 24.00,
                    Price100 = 22.00,
                    CategoryId = 1,
                    ImageUrl = ""
                },

                new Product
                {
                    Id = 7,
                    CreatedAt = new DateTime(2026, 1, 7),
                    Title = "The Hobbit",
                    Description = "Bilbo Baggins embarks on an unexpected journey with a group of dwarves.",
                    ISBN = "9780547928227",
                    Author = "J.R.R. Tolkien",
                    ListPrice = 27.00,
                    Price = 25.00,
                    Price50 = 23.00,
                    Price100 = 21.00,
                    CategoryId = 1,
                    ImageUrl = ""
                },

                new Product
                {
                    Id = 8,
                    CreatedAt = new DateTime(2026, 1, 8),
                    Title = "Design Patterns",
                    Description = "Essential reusable object-oriented software design patterns.",
                    ISBN = "9780201633610",
                    Author = "Erich Gamma",
                    ListPrice = 50.00,
                    Price = 47.00,
                    Price50 = 44.00,
                    Price100 = 41.00,
                    CategoryId = 3,
                    ImageUrl = ""
                },

                new Product
                {
                    Id = 9,
                    CreatedAt = new DateTime(2026, 1, 9),
                    Title = "Think and Grow Rich",
                    Description = "A classic book focused on personal success, mindset, and achievement.",
                    ISBN = "9781585424337",
                    Author = "Napoleon Hill",
                    ListPrice = 20.00,
                    Price = 18.00,
                    Price50 = 16.00,
                    Price100 = 14.00,
                    CategoryId = 2,
                    ImageUrl = ""
                },

                new Product
                {
                    Id = 10,
                    CreatedAt = new DateTime(2026, 1, 10),
                    Title = "Introduction to Algorithms",
                    Description = "A comprehensive introduction to algorithms and computer science fundamentals.",
                    ISBN = "9780262046305",
                    Author = "Thomas H. Cormen",
                    ListPrice = 60.00,
                    Price = 56.00,
                    Price50 = 52.00,
                    Price100 = 48.00,
                    CategoryId = 3,
                    ImageUrl = ""
                }
            ); base.OnModelCreating(modelBuilder);
        }
    }
}

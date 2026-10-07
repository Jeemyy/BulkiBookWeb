using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BulkiBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataForProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1000);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1007);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1008);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1009);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "CreatedAt", "Description", "ISBN", "ImageUrl", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "Alex Michaelides", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A psychological thriller about a famous painter who suddenly stops speaking.", "9781250301697", "", 25.0, 23.0, 19.0, 21.0, "The Silent Patient" },
                    { 2, "James Clear", 2, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "A practical guide to building good habits and breaking bad ones.", "9780735211292", "", 30.0, 28.0, 24.0, 26.0, "Atomic Habits" },
                    { 3, "Robert C. Martin", 3, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "A handbook of agile software craftsmanship and writing maintainable code.", "9780132350884", "", 45.0, 42.0, 36.0, 39.0, "Clean Code" },
                    { 4, "David Thomas", 3, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Practical advice and techniques for becoming a better software developer.", "9780135957059", "", 40.0, 37.0, 31.0, 34.0, "The Pragmatic Programmer" },
                    { 5, "Robert T. Kiyosaki", 2, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "A personal finance book about financial education and investing.", "9781612681139", "", 22.0, 20.0, 16.0, 18.0, "Rich Dad Poor Dad" },
                    { 6, "J.K. Rowling", 1, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "The first adventure of Harry Potter at Hogwarts School of Witchcraft and Wizardry.", "9780590353427", "", 28.0, 26.0, 22.0, 24.0, "Harry Potter and the Sorcerer's Stone" },
                    { 7, "J.R.R. Tolkien", 1, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bilbo Baggins embarks on an unexpected journey with a group of dwarves.", "9780547928227", "", 27.0, 25.0, 21.0, 23.0, "The Hobbit" },
                    { 8, "Erich Gamma", 3, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Essential reusable object-oriented software design patterns.", "9780201633610", "", 50.0, 47.0, 41.0, 44.0, "Design Patterns" },
                    { 9, "Napoleon Hill", 2, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "A classic book focused on personal success, mindset, and achievement.", "9781585424337", "", 20.0, 18.0, 14.0, 16.0, "Think and Grow Rich" },
                    { 10, "Thomas H. Cormen", 3, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "A comprehensive introduction to algorithms and computer science fundamentals.", "9780262046305", "", 60.0, 56.0, 48.0, 52.0, "Introduction to Algorithms" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "CreatedAt", "Description", "ISBN", "ImageUrl", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1000, "Alex Michaelides", 1, new DateTime(2026, 10, 7, 11, 32, 49, 786, DateTimeKind.Utc).AddTicks(2183), "A psychological thriller about a famous painter who suddenly stops speaking.", "9781250301697", "", 25.0, 23.0, 19.0, 21.0, "The Silent Patient" },
                    { 1001, "James Clear", 2, new DateTime(2026, 10, 7, 11, 32, 49, 786, DateTimeKind.Utc).AddTicks(4523), "A practical guide to building good habits and breaking bad ones.", "9780735211292", "", 30.0, 28.0, 24.0, 26.0, "Atomic Habits" },
                    { 1002, "Robert C. Martin", 3, new DateTime(2026, 10, 7, 11, 32, 49, 786, DateTimeKind.Utc).AddTicks(4538), "A handbook of agile software craftsmanship and writing maintainable code.", "9780132350884", "", 45.0, 42.0, 36.0, 39.0, "Clean Code" },
                    { 1003, "David Thomas", 3, new DateTime(2026, 10, 7, 11, 32, 49, 786, DateTimeKind.Utc).AddTicks(4540), "Practical advice and techniques for becoming a better software developer.", "9780135957059", "", 40.0, 37.0, 31.0, 34.0, "The Pragmatic Programmer" },
                    { 1004, "Robert T. Kiyosaki", 2, new DateTime(2026, 10, 7, 11, 32, 49, 786, DateTimeKind.Utc).AddTicks(4541), "A personal finance book about financial education and investing.", "9781612681139", "", 22.0, 20.0, 16.0, 18.0, "Rich Dad Poor Dad" },
                    { 1005, "J.K. Rowling", 1, new DateTime(2026, 10, 7, 11, 32, 49, 786, DateTimeKind.Utc).AddTicks(4543), "The first adventure of Harry Potter at Hogwarts School of Witchcraft and Wizardry.", "9780590353427", "", 28.0, 26.0, 22.0, 24.0, "Harry Potter and the Sorcerer's Stone" },
                    { 1006, "J.R.R. Tolkien", 1, new DateTime(2026, 10, 7, 11, 32, 49, 786, DateTimeKind.Utc).AddTicks(4544), "Bilbo Baggins embarks on an unexpected journey with a group of dwarves.", "9780547928227", "", 27.0, 25.0, 21.0, 23.0, "The Hobbit" },
                    { 1007, "Erich Gamma", 3, new DateTime(2026, 10, 7, 11, 32, 49, 786, DateTimeKind.Utc).AddTicks(4546), "Essential reusable object-oriented software design patterns.", "9780201633610", "", 50.0, 47.0, 41.0, 44.0, "Design Patterns" },
                    { 1008, "Napoleon Hill", 2, new DateTime(2026, 10, 7, 11, 32, 49, 786, DateTimeKind.Utc).AddTicks(4547), "A classic book focused on personal success, mindset, and achievement.", "9781585424337", "", 20.0, 18.0, 14.0, 16.0, "Think and Grow Rich" },
                    { 1009, "Thomas H. Cormen", 3, new DateTime(2026, 10, 7, 11, 32, 49, 786, DateTimeKind.Utc).AddTicks(4549), "A comprehensive introduction to algorithms and computer science fundamentals.", "9780262046305", "", 60.0, 56.0, 48.0, 52.0, "Introduction to Algorithms" }
                });
        }
    }
}

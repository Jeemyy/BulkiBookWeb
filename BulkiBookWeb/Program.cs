using BulkiBook.Business;
using BulkiBook.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace BulkiBookWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddBusiness();
            

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
               name: "MyArea",
               pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
               .WithStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}",
                defaults: new { area = "Customer" }
                )
                .WithStaticAssets();
            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}",
            //    defaults: new { area = "Admin" }
            //    ).WithStaticAssets();

            app.Run();
        }
    }
}

using BulkiBook.Business.Interfaces;
using BulkiBook.Business.Services;
using BulkyBook.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BulkiBook.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<IFileServices, FileServices>();
            services.AddScoped<IShoppingCartServices, ShoppingCartServices>();
            services.AddScoped<IApplicationUserServices, ApplicationUserServices>();

            return services;
        }
    }
}

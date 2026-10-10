using BulkiBook.Business.Interfaces;
using BulkiBook.Business.Services;
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
            return services;
        }
    }
}

using ArtGallery.Infrastructure.Data.Repositories;
using ArtGallery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Core.Contracts;
using ArtGallery.Core.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbRepository, ApplicationDbRepository>();
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IFileService, FileService>();
            //services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }

        public static IServiceCollection AddApplicationDbContexts(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
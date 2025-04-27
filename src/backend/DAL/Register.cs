using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Abstract;
using DAL.Repositories.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class Register
    {
        public static IServiceCollection AddAppData(this IServiceCollection services, string appCs)
        {
            services.AddDbContext<AppDbContext>(conf =>
            {
                conf.UseSqlServer(appCs,
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5, // Maksimum tekrar sayısı
                        maxRetryDelay: TimeSpan.FromSeconds(10), // Tekrarlar arası bekleme süresi
                        errorNumbersToAdd: null // Hangi hata kodlarında tekrar deneneceği (null ile varsayılan kullanılır)
                    );
                });
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductMetaTagsRepository, ProductMetaTagsRepository>();
            services.AddScoped<IContentRepository, ContentRepository>();
            services.AddScoped<IContentMediaRepository, ContentMediaRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            return services;
        }

    }
}

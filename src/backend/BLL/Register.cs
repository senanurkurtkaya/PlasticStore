using BLL.AbstractServices;
using BLL.ConcreteServices;
using DAL;
using DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class Register
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IProductMetaTagsService, ProductMetaTagsService>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IProductImageService, ProductImageService>();

            services.AddScoped<IFaqService, FaqService>();
            services.AddScoped<IFaqCategoryService, FaqCategoryService>();

            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<IContentMediaService, ContentMediaService>();


            services.AddAppData(connectionString);
            return services;


        }
    }
}

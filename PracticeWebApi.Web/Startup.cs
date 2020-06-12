using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticeWebApi.CommonClasses.Products;
using PracticeWebApi.CommonClasses.Users;
using PracticeWebApi.Data;
using PracticeWebApi.Data.Products;
using PracticeWebApi.Data.Users;
using PracticeWebApi.Services;
using PracticeWebApi.Services.Products;
using PracticeWebApi.Services.Users;

namespace PracticeWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IProductGroupService, ProductGroupService>();
            services.AddSingleton<IProductService, ProductService>();

            // change repo here
            services.AddSingleton<IProductGroupRepository, ProductGroupRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IMapper<User, UserDataEntity>, UserMapper>();
            services.AddSingleton<IMapper<ProductGroup, ProductGroupDataEntity>, ProductGroupMapper>();
            services.AddSingleton<IMapper<Product, ProductDataEntity>, ProductMapper>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

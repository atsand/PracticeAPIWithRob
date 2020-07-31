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
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //config
            var databaseConfiguration = new DatabaseConfiguration();
            Configuration.GetSection("DatabaseConfiguration").Bind(databaseConfiguration);
            services.AddSingleton(databaseConfiguration);

            // repositories - adding flag for using InMemory or not
            switch (databaseConfiguration.RepositoryType)
            {
                case "InMemory":
                    services.AddSingleton<IUserRepository, FakeUserRepository>();
                    services.AddSingleton<IProductGroupRepository, FakeProductGroupRepository>();
                    services.AddSingleton<IProductRepository, FakeProductRepository>();
                    break;
                default:
                    services.AddSingleton<IUserRepository, UserRepository>();
                    services.AddSingleton<IProductGroupRepository, ProductGroupRepository>();
                    services.AddSingleton<IProductRepository, ProductRepository>();
                    break;
            }

            //users
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IMapper<User, UserDataEntity>, UserMapper>();

            //product groups
            services.AddSingleton<IProductGroupService, ProductGroupService>();
            services.AddSingleton<IProductGroupRepository, ProductGroupRepository>();
            services.AddSingleton<IMapper<ProductGroup, ProductGroupDataEntity>, ProductGroupMapper>();

            //products
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IMapper<Product, ProductDataEntity>, ProductMapper>();

            //add cors policy to allow cross site requests
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
                 {
                     builder
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowAnyOrigin()
                         .SetIsOriginAllowed((host) => true);
                 });
            });
            services.AddControllers();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
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
            //add cors policy
            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

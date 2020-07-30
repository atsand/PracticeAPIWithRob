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
using PracticeWebApi.Services.Orders;
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
            //var databaseConfiguration = new DatabaseConfiguration();
            //Configuration.GetSection("DatabaseConfiguration").Bind(databaseConfiguration);
            //services.AddSingleton(databaseConfiguration);
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IProductGroupService, ProductGroupService>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IOrderService, OrderService>();

            // change repo here
            services.AddSingleton<IProductGroupRepository, ProductGroupRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            //update from fake repo
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IMapper<User, UserDataEntity>, UserMapper>();
            services.AddSingleton<IMapper<ProductGroup, ProductGroupDataEntity>, ProductGroupMapper>();
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
                         //.AllowCredentials();
                 });
            });
            //services.AddMvc();
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
            //app.UseCorsMiddleware();
            app.UseAuthorization();
            app.UseAuthentication();
            //app.UseMvc();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

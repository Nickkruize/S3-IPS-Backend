using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DAL;
using Microsoft.OpenApi.Models;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Interfaces;
using Services;
using S3_webshop.Hubs;
using DAL.ContextModels;

namespace S3_webshop
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("ClientPermission", policy =>
                {
                    policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:3000")
                    .AllowCredentials();
                });
            });

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("Anything", policy =>
            //    {
            //        policy.AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .AllowAnyOrigin()
            //        .AllowCredentials();
            //    });
            //});

            services.AddSignalR();

            if (Environment.IsDevelopment())
            {
                services.AddDbContext<WebshopContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LocalDb")));
            }
            else
            {
                services.AddDbContext<WebshopContext>(optionsbuilder =>
                {
                    optionsbuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                });
            }
            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<IProductRepo, ProductRepo>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryRepo, CategoryRepo>();
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<IUserService, UserService>();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebShop", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebShop v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("ClientPermission");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/hubs/chat");
            });


            Seed.InitializeCategories(app);
            Seed.InitializeProducts(app);
        }
    }
}

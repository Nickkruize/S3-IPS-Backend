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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using DAL.Helpers;

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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

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
            services.AddScoped<IJwtService, JwtService>();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebShop", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            UpdateDatabase(app, Environment);

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebShop v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("ClientPermission");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/hubs/chat");
            });
        }

        private async static void UpdateDatabase(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            WebshopContext context = serviceScope.ServiceProvider.GetService<WebshopContext>();
            context.Database.Migrate();

            if (env.IsDevelopment())
            {
                if (!context.Categories.Any())
                {
                    await context.Categories.AddRangeAsync(Seed.SeedCategories());
                    await context.SaveChangesAsync();
                }

                if (!context.Products.Any())
                {
                    List<Category> categories = context.Categories.ToList();
                    await context.Products.AddRangeAsync(Seed.SeedProducts(categories));
                    await context.SaveChangesAsync();
                }

                if (!context.Users.Any())
                {
                    await context.Users.AddAsync(Seed.SeedUser());
                    await context.SaveChangesAsync();
                }

                if (!context.OrderItems.Any())
                {
                    List<Product> products = context.Products.ToList();
                    User user = context.Users.Find(1);
                    List<Order> orders = Seed.SeedOrders(user);
                    await context.Orders.AddRangeAsync(orders);
                    await context.OrderItems.AddRangeAsync(Seed.SeedOrderItems(products,orders));
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

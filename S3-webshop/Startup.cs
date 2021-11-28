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
using Microsoft.AspNetCore.Identity;

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
                    .WithOrigins("http://localhost:3000", "http://192.168.178.115:3000")
                    .AllowCredentials();
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(jwt =>
                {
                    var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Secret"]);

                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        RequireExpirationTime = false
                    };
                });

            services.AddIdentityCore<IdentityUser>(options =>
            options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<WebshopContext>();

            services.AddSignalR();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("MustHaveId", policy => policy.RequireClaim("Id", "5fb37a20-76ab-402e-bb68-aaf32bdc2eaa"));
            //});

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

            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<IOrderItemRepo, OrderItemRepo>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();
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

                if (!context.Roles.Any())
                {
                    using var Scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                    RoleManager<IdentityRole> roleManager = Scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                    foreach (IdentityRole role in Seed.SeedRoles())
                    {
                        await roleManager.CreateAsync(role);
                    }
                }

                if (!context.Users.Any())
                {
                    using var Scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                    UserManager<IdentityUser> userManager = Scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
                    RoleManager<IdentityRole> roleManager = Scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                    List<User> Users = Seed.SeedUser();
                    foreach (User user in Users)
                    {
                        IdentityUser identityUser = new IdentityUser()
                        {
                            Email = user.Email,
                            UserName = user.Username
                        };

                        await userManager.CreateAsync(identityUser, user.Password);
                        if (user.Username == "Admin")
                        {
                            await userManager.AddToRoleAsync(identityUser, "Admin");
                        }
                        else
                        {
                            await userManager.AddToRoleAsync(identityUser, "User");
                        }
                    }
                }

                if (!context.OrderItems.Any())
                {
                    List<Product> products = context.Products.ToList();
                    List<IdentityUser> users = context.Users.ToList();
                    List<Order> orders = Seed.SeedOrders(users);
                    await context.Orders.AddRangeAsync(orders);
                    await context.OrderItems.AddRangeAsync(Seed.SeedOrderItems(products, orders));
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

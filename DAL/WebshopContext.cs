using DAL.ContextModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    public class WebshopContext: DbContext
    {
        public WebshopContext(DbContextOptions<WebshopContext> options): base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}

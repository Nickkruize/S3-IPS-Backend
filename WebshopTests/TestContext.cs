using DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebshopTests
{
    public class TestContext
    {
        public WebshopContext SqlLiteInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<WebshopContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var context = new WebshopContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            return context;
        }
    }
}

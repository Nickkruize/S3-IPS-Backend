using DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebshopTests
{
    public class TestContext
    {
        public async Task<WebshopContext> GetDatabaseContext<T>(List<T> data, string dbName) where T : class
        {
            var options = new DbContextOptionsBuilder<WebshopContext>().UseInMemoryDatabase(databaseName: dbName).EnableSensitiveDataLogging().Options;
            var databaseContext = new WebshopContext(options);
            databaseContext.Database.EnsureCreated();

            if (await databaseContext.Set<T>().CountAsync() <= 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    databaseContext.Set<T>().Add(data[i]);
                }
                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }

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

using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}

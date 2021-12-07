using DAL.ContextModels;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebshopTests.RepositoryTests
{
    [TestClass]
    public class ProductRepoTest : TestContext
    {
        public ProductRepoTest()
        {
        }

        [TestMethod]
        public async Task Itaddsaproduct()
        {
            ProductRepo repo = new ProductRepo(SqlLiteInMemoryContext());
            Product product = new Product
            {
                Description = "coole beschrijving",
                ImgUrl = "random image",
                Name = "testproduct",
                Price = 9.99
            };

            await repo.Create(product);
            await repo.Save();

            Product savedProduct = await repo.GetById(1);

            Assert.IsTrue(product.IsDeepEqual(savedProduct));
        }
    }
}

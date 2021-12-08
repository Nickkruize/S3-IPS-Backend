using DAL.ContextModels;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebshopTests.ServiceTests
{
    [TestClass]
    public class ProductServiceTest
    {
        public ProductServiceTest()
        {
            TestProductRepository testProductRepository = new TestProductRepository();
        }

        [TestMethod]
        public async Task ItReturnsAProductWhenAValidIdIsGiven()
        {
            TestProductRepository testProductRepository = new TestProductRepository();
            Product result = await testProductRepository.GetById(1);

            Product expected = new Product
            {
                Id = 1,
                Description = "coole beschrijving",
                ImgUrl = "random image",
                Name = "testproduct",
                Price = 9.99
            };

            Assert.IsTrue(result.IsDeepEqual(expected));
        }

        [TestMethod]
        public async Task ItReturnsNullWhenInvalidId()
        {
            TestProductRepository testProductRepository = new TestProductRepository();
            Product result = await testProductRepository.GetById(10);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task ItReturnsTheListWithProducts()
        {
            TestProductRepository testProductRepository = new TestProductRepository();
            List<Product> result = await testProductRepository.FindAll() as List<Product>;

            List<Product> expected = new List<Product>();
            Product product1 = new Product
            {
                Id = 1,
                Description = "coole beschrijving",
                ImgUrl = "random image",
                Name = "testproduct",
                Price = 9.99
            };
            Product product2 = new Product
            {
                Id = 2,
                Description = "nieuwe beschrijving",
                ImgUrl = "random image",
                Name = "testproduct2",
                Price = 99.99
            };
            expected.Add(product1);
            expected.Add(product2);

            Assert.IsNotNull(result.Count);
            Assert.IsTrue(result.IsDeepEqual(expected));
        }
    }
}

using DAL.ContextModels;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repositories.Interfaces;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopTests.ServiceTests
{
    [TestClass]
    public class ProductServiceTest
    {
        public ProductServiceTest()
        {
        }

        private List<Product> GetProducts()
        {
            Product product = new Product
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

            List<Product> Products = new List<Product>
            {
                product, product2
            };

            return Products;
        }

        [TestMethod]
        public async Task ItReturnsAProductWhenAValidIdIsGiven()
        {
            var testProductRepository = new Mock<IProductRepo>();
            testProductRepository.Setup(arg => arg.GetById(It.IsAny<int>()))
                .ReturnsAsync((int i) => GetProducts().Single(c => c.Id == i));

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            Product result = await service.GetById(1);

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
            var testProductRepository = new Mock<IProductRepo>();
            testProductRepository.Setup(arg => arg.GetById(It.IsAny<int>()))
                .ReturnsAsync((int i) => GetProducts().FirstOrDefault(c => c.Id == i));

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            Product result = await service.GetById(10);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task ItReturnsTheListWithProducts()
        {
            var testProductRepository = new Mock<IProductRepo>();
            testProductRepository.Setup(arg => arg.FindAllWithProductCategories())
                .ReturnsAsync(GetProducts());

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            List<Product> result = await service.GetAllWithCategories() as List<Product>;

            List<Product> expected = GetProducts();

            Assert.IsNotNull(result.Count);
            Assert.IsTrue(result.IsDeepEqual(expected));
        }
    }
}

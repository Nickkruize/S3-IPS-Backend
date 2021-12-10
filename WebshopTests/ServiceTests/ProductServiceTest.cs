using DAL.ContextModels;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repositories.Interfaces;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

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
                Price = 9.99,
                Categories = GetCategory(new List<int> { 1, 2 })
            };
            Product product2 = new Product
            {
                Id = 2,
                Description = "nieuwe beschrijving",
                ImgUrl = "random image",
                Name = "testproduct2",
                Price = 99.99,
                Categories = GetCategory(new List<int>())
            };

            List<Product> Products = new List<Product>
            {
                product, product2
            };

            return Products;
        }

        private List<Category> GetCategory(List<int> i)
        {
            List<Category> categories = new List<Category>();
            foreach (int id in i)
            {
                categories.Add(new Category
                {
                    Id = id,
                    ImgUrl = "imgUrl" + id,
                    Name = "Category" + id,
                });
            }

            return categories;
        }

        private Category GetCategory(int id)
        {
            return new Category
                {
                    Id = id,
                    ImgUrl = "imgUrl" + id,
                    Name = "Category" + id,
                };           
        }
        [TestMethod]
        public async Task GetByIdReturnsAProductWhenAValidIdIsGiven()
        {
            var testProductRepository = new Mock<IProductRepo>();
            testProductRepository.Setup(arg => arg.GetById(It.IsAny<int>()))
                .ReturnsAsync((int i) => GetProducts().FirstOrDefault(c => c.Id == i));

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            Product result = await service.GetById(1);
            result.Categories = null;

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
        public async Task GetByIdReturnsNullWhenInvalidId()
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
        public async Task GetProductsReturnsTheListWithProducts()
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

        [TestMethod]
        public async Task GetByIdWithCategoriesReturnsAProductWithCategories()
        {
            var testProductRepository = new Mock<IProductRepo>();
            testProductRepository.Setup(arg => arg.FindByIdWithCategoires(It.IsAny<int>()))
                .ReturnsAsync((int i) => GetProducts().FirstOrDefault(c => c.Id == i));

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            Product result = await service.GetByIdWithCategories(1);

            Product expected = new Product
            {
                Id = 1,
                Description = "coole beschrijving",
                ImgUrl = "random image",
                Name = "testproduct",
                Price = 9.99,
                Categories = GetCategory(new List<int> { 1, 2 })
            };

            Assert.IsTrue(result.IsDeepEqual(expected));
        }

        [TestMethod]
        public async Task GetByIdWithCategoriesReturnsNullWhenInvalidId()
        {
            var testProductRepository = new Mock<IProductRepo>();
            testProductRepository.Setup(arg => arg.FindByIdWithCategoires(It.IsAny<int>()))
                .ReturnsAsync((int i) => GetProducts().FirstOrDefault(c => c.Id == i));

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            Product result = await service.GetByIdWithCategories(10);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteSucces()
        {
            List<Product> products = GetProducts();
            var testProductRepository = new Mock<IProductRepo>();
            testProductRepository.Setup(arg => arg.Delete(It.IsAny<Product>()))
                .Callback((Product p) => products.Remove(p));

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            Product productToDelete = products[0];
            Product result = await service.Delete(productToDelete);

            Assert.AreEqual(productToDelete, result);
            Assert.AreEqual(1, products.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Fail")]
        public async Task DeleteFail()
        {
            var testProductRepository = new Mock<IProductRepo>();
            testProductRepository.Setup(arg => arg.Delete(It.IsAny<Product>()))
                .Throws(new Exception("Fail"));

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            Product productToDelete = new Product();
            await service.Delete(productToDelete);
        }

        [TestMethod]
        public async Task AddProductSucces()
        {
            List<Product> products = GetProducts();
            var testProductRepository = new Mock<IProductRepo>();
            testProductRepository.Setup(arg => arg.Create(It.IsAny<Product>()))
                .Callback((Product p) => products.Add(p))
                .ReturnsAsync((Product p) => p);

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            Product productToAdd = new Product
            {
                Id = 3,
                ImgUrl = "new",
                Description = "new",
                Name = "new",
                Price = 10.10
            };

            Product result = await service.AddProduct(productToAdd);

            Assert.AreEqual(productToAdd, result);
            Assert.AreEqual(3, products.Count);
            Assert.IsTrue(productToAdd.IsDeepEqual(products[2]));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Failed")]
        public async Task AddProductFailsWhenAdding()
        {
            var testProductRepository = new Mock<IProductRepo>();
            testProductRepository.Setup(arg => arg.Create(It.IsAny<Product>()))
                .Throws(new Exception("Failed"));

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            Product productToAdd = new Product
            {
                Id = 3,
                ImgUrl = "new",
                Description = "new",
                Name = "new",
                Price = 10.10
            };

            await service.AddProduct(productToAdd);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Failed")]
        public async Task AddProductFailsWhenSaving()
        {
            var testProductRepository = new Mock<IProductRepo>();
            testProductRepository.Setup(arg => arg.Save())
                .Throws(new Exception("Failed"));

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            Product productToAdd = new Product
            {
                Id = 3,
                ImgUrl = "new",
                Description = "new",
                Name = "new",
                Price = 10.10
            };

            await service.AddProduct(productToAdd);
        }

        [TestMethod]
        public async Task UpdateProductSucces()
        {
            List<Product> products = GetProducts();
            var testProductRepository = new Mock<IProductRepo>();
            testProductRepository.Setup(arg => arg.Update(It.IsAny<Product>()))
                .Callback((Product p) => products[products.FindIndex(e => e.Id == p.Id)] = p);

            var categoryRepository = new Mock<ICategoryRepo>();
            categoryRepository.Setup(arg => arg.GetById(It.IsAny<int>())).ReturnsAsync((int s) => GetCategory(s));

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            Product productToUpdate = new Product
            {
                Id = 2,
                ImgUrl = "new",
                Description = "new",
                Name = "new",
                Price = 10.10,
                Categories = GetCategory(new List<int> { 2 })
            };

            await service.Update(productToUpdate, 1);

            Assert.AreEqual(2, products.Count);
            Assert.IsTrue(productToUpdate.IsDeepEqual(products[1]));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Failed")]
        public async Task UpdateProductFailsWhenSaving()
        {
            var testProductRepository = new Mock<IProductRepo>();
            testProductRepository.Setup(arg => arg.Save())
                .Throws(new Exception("Failed"));

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            Product productToAdd = new Product
            {
                Id = 3,
                ImgUrl = "new",
                Description = "new",
                Name = "new",
                Price = 10.10
            };

            await service.Update(productToAdd, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Failed")]
        public async Task UpdateProductFailsWhenUpdating()
        {
            var testProductRepository = new Mock<IProductRepo>();
            testProductRepository.Setup(arg => arg.Update(It.IsAny<Product>()))
                .Throws(new Exception("Failed"));

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            Product productToAdd = new Product
            {
                Id = 3,
                ImgUrl = "new",
                Description = "new",
                Name = "new",
                Price = 10.10
            };

            await service.Update(productToAdd, 3);
        }

        [TestMethod]
        public async Task AppendingCategoriesToProductSucceeds()
        {
            List<Product> products = GetProducts();
            List<Category> categories = GetCategory(new List<int> { 1, 2 });
            var testProductRepository = new Mock<IProductRepo>();

            var categoryRepository = new Mock<ICategoryRepo>();
            categoryRepository.Setup(arg => arg.FindByCondition(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(categories);

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            Product expected = products[1];
            expected.Categories = categories;

            Product result = await service.AppendCategoriesToProduct(new List<int> { 1, 2 }, products[1]);

            Assert.IsTrue(expected.IsDeepEqual(result));
        }

        [TestMethod]
        public void VerifyAllSubmittedCategoriesWhereFoundSucces()
        {
            List<Product> products = GetProducts();
            var testProductRepository = new Mock<IProductRepo>();

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            bool result = service.VerifyAllSubmittedCategoriesWhereFound(products[0], new List<int> { 1, 2 });
            bool result2 = service.VerifyAllSubmittedCategoriesWhereFound(products[1], new List<int>());

            Assert.IsTrue(result);
            Assert.IsTrue(result2);
        }

        [TestMethod]
        public void VerifyAllSubmittedCategoriesNotFoundReturnsFalse()
        {
            List<Product> products = GetProducts();
            var testProductRepository = new Mock<IProductRepo>();

            var categoryRepository = new Mock<ICategoryRepo>();

            ProductService service = new ProductService(testProductRepository.Object, categoryRepository.Object);

            bool result = service.VerifyAllSubmittedCategoriesWhereFound(products[1], new List<int> { 1, 2 });
            bool result2 = service.VerifyAllSubmittedCategoriesWhereFound(products[0], new List<int>());

            Assert.IsFalse(result);
            Assert.IsFalse(result2);
        }




    }
}

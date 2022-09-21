using AutoMapper;
using DAL.ContextModels;
using DeepEqual.Syntax;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using S3_webshop.Configuration;
using S3_webshop.Controllers;
using S3_webshop.Resources;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebshopTests.ControllerTests
{
    [TestClass]
    public class ProductControllerTest : TestContext
    {
        private readonly IMapper _mapper;

        public ProductControllerTest()
        {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MapperProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
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

            return new List<Product>
            {
                product, product2
            };
        }

        private List<Category> GetCategories()
        {
            Category category1 = new Category
            {
                Id = 1,
                ImgUrl = "imgurl1",
                Name = "Category1",
                Products = null
            };
            Category category2 = new Category
            {
                Id = 2,
                ImgUrl = "imgurl2",
                Name = "Category2",
                Products = null
            };

            return new List<Category>() { category1, category2 };
        }

        [TestMethod]
        public async Task ItReturnAListOfProducts()
        {
            var categoryService = new Mock<ICategoryService>();
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.GetAllWithCategories())
                .ReturnsAsync(GetProducts());
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            List<ProductWithCategoriesResource> productResources = _mapper.Map<List<Product>, List<ProductWithCategoriesResource>>(GetProducts()); ;

            var controllerResult = await controller.Get();
            Assert.IsInstanceOfType(controllerResult.Result, typeof(OkObjectResult));
            var actual = (controllerResult.Result as OkObjectResult).Value as List<ProductWithCategoriesResource>;
            Assert.IsTrue(productResources.IsDeepEqual(actual));
        }

        [TestMethod]
        public async Task GetReturnsA500CodeWithAnErrorMessage()
        {
            var categoryService = new Mock<ICategoryService>();
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.GetAllWithCategories())
                .ThrowsAsync(new InvalidOperationException());
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            var actionResult = await controller.Get();
            var result = actionResult.Result as ObjectResult;
            Assert.IsInstanceOfType(actionResult.Result, typeof(ObjectResult));
            Assert.AreEqual(500, result.StatusCode);
            Assert.IsNotNull(result.Value);
        }

        [TestMethod]
        public async Task ItReturnsOkResultWithCorrectDataForGetId()
        {
            var categoryService = new Mock<ICategoryService>();
            List<Product> products = GetProducts();
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.GetByIdWithCategories(It.IsAny<int>()))
                .ReturnsAsync((int i) => products.Single(c => c.Id == i));
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            ProductWithCategoriesResource product1Resource = _mapper.Map<Product, ProductWithCategoriesResource>(products[0]);
            ProductWithCategoriesResource product2Resource = _mapper.Map<Product, ProductWithCategoriesResource>(products[1]);

            var controllerResult1 = await controller.Get(1);
            var controllerResult2 = await controller.Get(2);
            Assert.IsInstanceOfType(controllerResult1.Result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(controllerResult2.Result, typeof(OkObjectResult));
            var actual = (controllerResult1.Result as OkObjectResult).Value as ProductWithCategoriesResource;
            var actual2 = (controllerResult2.Result as OkObjectResult).Value as ProductWithCategoriesResource;
            Assert.IsTrue(product1Resource.IsDeepEqual(actual));
            Assert.IsTrue(product2Resource.IsDeepEqual(actual2));
        }

        [TestMethod]
        public async Task ItReturnsNotFoundForNonExistingId()
        {
            var categoryService = new Mock<ICategoryService>();
            var service = new Mock<IProductService>();
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            var controllerResult = await controller.Get(3);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetByIdReturnsA500CodeWithAnErrorMessage()
        {
            var categoryService = new Mock<ICategoryService>();
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.GetByIdWithCategories(It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException());
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            var actionResult = await controller.Get(1);
            Assert.IsNull(actionResult.Value);
            Assert.IsInstanceOfType(actionResult.Result, typeof(ObjectResult));
            var result = actionResult.Result as ObjectResult;
            Assert.AreEqual(500, result.StatusCode);
            Assert.IsNotNull(result.Value);
        }

        [TestMethod]
        public async Task PostReturnsBadRequestWhenModelStateIsInvalid()
        {
            var categoryService = new Mock<ICategoryService>();
            var service = new Mock<IProductService>();
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            controller.ModelState.AddModelError("error", "error");
            IActionResult result = await controller.Post(new NewProductResource());

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var result2 = result as BadRequestObjectResult;
            Assert.AreEqual(400, result2.StatusCode);
            Assert.AreEqual("Invalid information", result2.Value);
        }

        [TestMethod]
        public async Task PostReturns500WhenAppendingCategoriesFails()
        {
            var categoryService = new Mock<ICategoryService>();
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.AppendCategoriesToProduct(It.IsAny<List<int>>(), It.IsAny<Product>()))
                .ThrowsAsync(new InvalidOperationException("Action failed"));
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            NewProductResource input = new NewProductResource
            {
                Name = "",
                Description = "test description",
                Price = 9.99,
                CategoryIds = new List<int>{
                    1,2,3
                }
            };

            var result = await controller.Post(input) as ObjectResult;
            Assert.AreEqual(500, result.StatusCode);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("Action failed", result.Value);
        }

        [TestMethod]
        public async Task PostReturnsBadRequestIfAnyCategoriesWhereNotFound()
        {
            var categoryService = new Mock<ICategoryService>();
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.VerifyAllSubmittedCategoriesWhereFound(It.IsAny<Product>(), It.IsAny<List<int>>()))
                .Returns(false);
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            NewProductResource input = new NewProductResource
            {
                Name = "",
                Description = "test description",
                Price = 9.99,
                CategoryIds = new List<int>{
                    1,2,3
                }
            };

            var result = await controller.Post(input) as ObjectResult;
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("One or more invalid CategoryIds", result.Value);
        }

        [TestMethod]
        public async Task PostReturns500IfAddingProductFails()
        {
            var categoryService = new Mock<ICategoryService>();
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.AddProduct(It.IsAny<Product>()))
                .ThrowsAsync(new InvalidOperationException("Product could not be added"));
            service.Setup(arg => arg.VerifyAllSubmittedCategoriesWhereFound(It.IsAny<Product>(), It.IsAny<List<int>>()))
                .Returns(true);
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            NewProductResource input = new NewProductResource
            {
                Name = "",
                Description = "test description",
                Price = 9.99,
                CategoryIds = new List<int>{
                    1,2,3
                }
            };

            var result = await controller.Post(input) as ObjectResult;
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual("Product could not be added", result.Value);
        }

        [TestMethod]
        public async Task PostReturns201IfProductIsAddedSuccesfully()
        {
            var categoryService = new Mock<ICategoryService>();
            Product product = GetProducts()[0];
            product.Categories = GetCategories();
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.AppendCategoriesToProduct(It.IsAny<List<int>>(), It.IsAny<Product>()))
                .ReturnsAsync(product);
            service.Setup(arg => arg.VerifyAllSubmittedCategoriesWhereFound(It.IsAny<Product>(), It.IsAny<List<int>>()))
                .Returns(true);
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            NewProductResource input = new NewProductResource
            {
                Name = "testproduct",
                Description = "coole beschrijving",
                Price = 9.99,
                CategoryIds = new List<int>{
                    1,2
                }
            };

            var result = await controller.Post(input) as ObjectResult;
            Assert.AreEqual(201, result.StatusCode);
            Assert.IsTrue(product.IsDeepEqual(result.Value));
        }

        [TestMethod]
        public async Task DeleteReturns500IfGetByIdFails()
        {
            var categoryService = new Mock<ICategoryService>();
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.GetById(It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("Database Error"));
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);


            var result = await controller.DeleteProduct(1) as ObjectResult;
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual("Database Error", result.Value);
        }

        [TestMethod]
        public async Task DeleteReturns500IfDeleteFails()
        {
            var categoryService = new Mock<ICategoryService>();
            List<Product> products = GetProducts();
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.Delete(It.IsAny<Product>()))
                .Throws(new InvalidOperationException("Database Error"));
            service.Setup(arg => arg.GetById(It.IsAny<int>()))
                .ReturnsAsync((int i) => products.FirstOrDefault(c => c.Id == i));
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            var result = await controller.DeleteProduct(1) as ObjectResult;
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual("Database Error", result.Value);
        }

        [TestMethod]
        public async Task DeleteReturns404IfProductIsNotFound()
        {
            var categoryService = new Mock<ICategoryService>();
            List<Product> products = GetProducts();
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.GetById(It.IsAny<int>()))
                .ReturnsAsync((int i) => products.FirstOrDefault(c => c.Id == i));
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            var result = await controller.DeleteProduct(5) as ObjectResult;
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("Product was not found", result.Value);
        }

        [TestMethod]
        public async Task DeleteReturns204IfDeleteSuccesful()
        {
            var categoryService = new Mock<ICategoryService>();
            List<Product> products = GetProducts();
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.GetById(It.IsAny<int>()))
                .ReturnsAsync((int i) => products.FirstOrDefault(c => c.Id == i));
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            var result = await controller.DeleteProduct(1) as NoContentResult;
            Assert.AreEqual(204, result.StatusCode);
        }

        //[TestMethod]
        //public async Task PutReturns201IfUpdateSuccesful()
        //{
        //    var categoryService = new Mock<ICategoryService>();
        //    List<Category> categories = GetCategories();
        //    categoryService.Setup(arg => arg.GetById(It.IsAny<int>()))
        //        .ReturnsAsync((int i) => categories.FirstOrDefault(c => c.Id == i));
        //    var service = new Mock<IProductService>();
        //    List<Product> products = GetProducts();
        //    service.Setup(arg => arg.GetByIdWithCategories(It.IsAny<int>()))
        //        .ReturnsAsync((int i) => products.FirstOrDefault(c => c.Id == i));
        //    ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

        //    List<CategoryResource> categoryResources = _mapper.Map<List<Category>, List<CategoryResource>>(GetCategories());
        //    ProductWithCategoriesResource input = new ProductWithCategoriesResource
        //    {
        //        Id = 1,
        //        imgUrl = "MooiProduct.jpg",
        //        Description = "beschrijving",
        //        Name = "Mooi Product",
        //        Price = 9.99,
        //        Categories = categoryResources
        //    };

        //    Product expected = _mapper.Map<ProductWithCategoriesResource, Product>(input);

        //    var result = await controller.Put(1, input, 1) as ObjectResult;
        //    Assert.AreEqual(201, result.StatusCode);
        //    Assert.IsTrue(expected.IsDeepEqual(result.Value));
        //}

        [TestMethod]
        public async Task PutReturnsBadRequestWhenModelStateIsInvalid()
        {
            var categoryService = new Mock<ICategoryService>();
            var service = new Mock<IProductService>();
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            controller.ModelState.AddModelError("error", "error");
            IActionResult result = await controller.Put(1, new ProductWithCategoriesResource(), 1);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var result2 = result as BadRequestObjectResult;
            Assert.AreEqual(400, result2.StatusCode);
            Assert.AreEqual("Invalid Information", result2.Value);
        }

        [TestMethod]
        public async Task PutReturns400IfIdDoesntMatchIdInModel()
        {
            var categoryService = new Mock<ICategoryService>();
            var service = new Mock<IProductService>();
            ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

            List<CategoryResource> categoryResources = _mapper.Map<List<Category>, List<CategoryResource>>(GetCategories());
            ProductWithCategoriesResource input = new ProductWithCategoriesResource
            {
                Id = 1,
                imgUrl = "MooiProduct.jpg",
                Description = "beschrijving",
                Name = "Mooi Product",
                Price = 9.99,
                Categories = categoryResources
            };

            var result = await controller.Put(2, input, 3) as BadRequestObjectResult;

            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("submitted Id doesn't match the productId", result.Value);
        }

        //[TestMethod]
        //public async Task PutReturnsBadRequestIfProductNotInDB()
        //{
        //    var categoryService = new Mock<ICategoryService>();
        //    List<Product> products = GetProducts();
        //    var service = new Mock<IProductService>();
        //    service.Setup(s => s.Update(It.IsAny<Product>(), It.IsAny<int>())).Throws(new DbUpdateConcurrencyException("Db Update Failed"));
        //    service.Setup(arg => arg.GetById(It.IsAny<int>()))
        //        .ReturnsAsync((int i) => products.FirstOrDefault(c => c.Id == i));
        //    ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

        //    List<CategoryResource> categoryResources = _mapper.Map<List<Category>, List<CategoryResource>>(GetCategories());
        //    ProductWithCategoriesResource input = new ProductWithCategoriesResource
        //    {
        //        Id = 10,
        //        imgUrl = "MooiProduct.jpg",
        //        Description = "beschrijving",
        //        Name = "Mooi Product",
        //        Price = 9.99,
        //        Categories = categoryResources
        //    };

        //    var result = await controller.Put(10, input, 4) as NotFoundObjectResult;

        //    Assert.AreEqual(404, result.StatusCode);
        //    Assert.AreEqual("This product doesn't exist", result.Value);
        //}

        //[TestMethod]
        //public async Task PutReturnsBadRequestIfDbUpdateFailedButIdExists()
        //{
        //    var categoryService = new Mock<ICategoryService>();
        //    List<Category> categories = GetCategories();
        //    categoryService.Setup(arg => arg.GetById(It.IsAny<int>()))
        //        .ReturnsAsync((int i) => categories.FirstOrDefault(c => c.Id == i));
        //    List<Product> products = GetProducts();
        //    var service = new Mock<IProductService>();
        //    service.Setup(s => s.Update(It.IsAny<Product>(), It.IsAny<int>())).Throws(new DbUpdateConcurrencyException("Db Update Failed"));
        //    service.Setup(arg => arg.GetById(It.IsAny<int>()))
        //        .ReturnsAsync((int i) => products.FirstOrDefault(c => c.Id == i));
        //    ProductController controller = new ProductController(service.Object, _mapper, categoryService.Object);

        //    List<CategoryResource> categoryResources = _mapper.Map<List<Category>, List<CategoryResource>>(GetCategories());
        //    ProductWithCategoriesResource input = new ProductWithCategoriesResource
        //    {
        //        Id = 1,
        //        imgUrl = "MooiProduct.jpg",
        //        Description = "beschrijving",
        //        Name = "Mooi Product",
        //        Price = 9.99,
        //        Categories = categoryResources
        //    };

        //    var result = await controller.Put(1, input, 4) as BadRequestObjectResult;

        //    Assert.AreEqual(400, result.StatusCode);
        //    Assert.AreEqual("Db Update Failed", result.Value);
        //}

    }
}

using AutoMapper;
using DAL.ContextModels;
using DeepEqual.Syntax;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repositories.Repositories;
using S3_webshop.Configuration;
using S3_webshop.Controllers;
using S3_webshop.Resources;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebshopTests.ControllerTests
{
    [TestClass]
    public class ProductControllerTest : TestContext
    {
        private readonly IMapper _mapper;

        public ProductControllerTest()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MapperProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [TestMethod]
        public async Task ItReturnAListOfProducts2()
        {
            TestProductService service = new TestProductService();
            ProductController controller = new ProductController(service, _mapper);
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

            List<ProductWithCategoryResource> productResources = _mapper.Map<List<Product>, List<ProductWithCategoryResource>>(Products);

            var result = await service.GetAllWithCategories();
            Assert.IsTrue(Products.IsDeepEqual(result));

            var controllerResult = await controller.Get();
            Assert.IsInstanceOfType(controllerResult.Result, typeof(OkObjectResult));
            var actual = (controllerResult.Result as OkObjectResult).Value as List<ProductWithCategoryResource>;
            Assert.IsTrue(productResources.IsDeepEqual(actual));
        }

        [TestMethod]
        public async Task ItReturnsOkResultWithCorrectDataForGetId()
        {
            TestProductService service = new TestProductService();
            ProductController controller = new ProductController(service, _mapper);
            Product product = new Product
            {
                Id = 1,
                Description = "coole beschrijving",
                ImgUrl = "random image",
                Name = "testproduct",
                Price = 9.99
            };


            ProductWithCategoriesResource productResource = _mapper.Map<Product, ProductWithCategoriesResource>(product);
            var result = await service.GetById(1);
            Assert.IsTrue(product.IsDeepEqual(result));

            var controllerResult = await controller.Get(1);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(OkObjectResult));
            var actual = (controllerResult.Result as OkObjectResult).Value as ProductWithCategoriesResource;
            Assert.IsTrue(productResource.IsDeepEqual(actual));
        }

        [TestMethod]
        public async Task ItReturnsNotFoundForNonExistingId()
        {
            TestProductService service = new TestProductService();
            ProductController controller = new ProductController(service, _mapper);

            var controllerResult = await controller.Get(3);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(NotFoundResult));
        }

    }
}

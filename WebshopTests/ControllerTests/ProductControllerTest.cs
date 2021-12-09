﻿using AutoMapper;
using DAL.ContextModels;
using DeepEqual.Syntax;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repositories.Repositories;
using S3_webshop.Configuration;
using S3_webshop.Controllers;
using S3_webshop.Resources;
using Services;
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
        public async Task ItReturnAListOfProducts()
        {
             var service = new Mock<IProductService>();
            service.Setup(arg => arg.GetAllWithCategories())
                .ReturnsAsync(GetProducts());
            ProductController controller = new ProductController(service.Object, _mapper);

            List<ProductWithCategoryResource> productResources = _mapper.Map<List<Product>, List<ProductWithCategoryResource>>(GetProducts()); ;

            var controllerResult = await controller.Get();
            Assert.IsInstanceOfType(controllerResult.Result, typeof(OkObjectResult));
            var actual = (controllerResult.Result as OkObjectResult).Value as List<ProductWithCategoryResource>;
            Assert.IsTrue(productResources.IsDeepEqual(actual));
        }

        [TestMethod]
        public async Task GetReturnsA500CodeWithAnErrorMessage()
        {
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.GetAllWithCategories())
                .ThrowsAsync(new InvalidOperationException());
            ProductController controller = new ProductController(service.Object, _mapper);

            var actionResult = await controller.Get();
            var result = actionResult.Result as ObjectResult;
            Assert.IsInstanceOfType(actionResult.Result, typeof(ObjectResult));
            Assert.AreEqual(500, result.StatusCode);
            Assert.IsNotNull(result.Value);
        }

        [TestMethod]
        public async Task ItReturnsOkResultWithCorrectDataForGetId()
        {
            List<Product> products = GetProducts();
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.GetByIdWithCategories(It.IsAny<int>()))
                .ReturnsAsync((int i) => products.Single(c => c.Id == i));
            ProductController controller = new ProductController(service.Object, _mapper);

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
            var service = new Mock<IProductService>();
            ProductController controller = new ProductController(service.Object, _mapper);

            var controllerResult = await controller.Get(3);
            Assert.IsInstanceOfType(controllerResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetByIdReturnsA500CodeWithAnErrorMessage()
        {
            var service = new Mock<IProductService>();
            service.Setup(arg => arg.GetByIdWithCategories(It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException());
            ProductController controller = new ProductController(service.Object, _mapper);

            var actionResult = await controller.Get(1);
            Assert.IsNull(actionResult.Value);
            Assert.IsInstanceOfType(actionResult.Result, typeof(ObjectResult));
            var result = actionResult.Result as ObjectResult;
            Assert.AreEqual(500, result.StatusCode);
            Assert.IsNotNull(result.Value);
        }
    }
}

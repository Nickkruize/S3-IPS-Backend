using Microsoft.VisualStudio.TestTools.UnitTesting;
using S3_webshop.Resources;
using S3_webshop.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace WebshopTests.ModelValidationTests
{
    [TestClass]
    public class NewProductResourceValidationTest
    {
        [TestMethod]
        public void ItValidatesModel()
        {
            NewProductResource input = new NewProductResource
            {
                Name = "New product",
                Description = "test description",
                Price = 9.99,           
                CategoryIds = new List<int>{
                    1,2,3
                }
            };

            var context = new ValidationContext(input, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(input, context, results, true);

            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void ItShowsErrorWhenNoNameProvided()
        {
            NewProductResource input = new NewProductResource
            {
                Name = "",
                Description = "test description",
                Price = 9.99,
                CategoryIds = new List<int>{
                    1,2,3
                }
            };

            var context = new ValidationContext(input, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(input, context, results, true);

            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("A product name must be provided", results[0].ErrorMessage);
        }

        [TestMethod]
        public void ItShowsErrorWhenNoDescriptionIsProvided()
        {
            NewProductResource input = new NewProductResource
            {
                Name = "New product",
                Description = "",
                Price = 9.99,
                CategoryIds = new List<int>{
                    1,2,3
                }
            };

            var context = new ValidationContext(input, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(input, context, results, true);

            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("A description of the product must be provided", results[0].ErrorMessage);
        }

        [TestMethod]
        public void ItShowsErrorWhenNoPriceIsProvided()
        {
            NewProductResource input = new NewProductResource
            {
                Name = "New product",
                Description = "test",
                CategoryIds = new List<int>{
                    1,2,3
                }
            };

            var context = new ValidationContext(input, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(input, context, results, true);

            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Price must be between 0.01 and 9999.99", results[0].ErrorMessage);
        }

        [TestMethod]
        public void ItShowsErrorWhenACategoryIdIsDuplicated()
        {
            NewProductResource input = new NewProductResource
            {
                Name = "New product",
                Description = "test",
                Price = 9.99,
                CategoryIds = new List<int>{
                    1,2,3,3
                }
            };

            var context = new ValidationContext(input, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(input, context, results, true);

            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("a categoryId can't be used multiple times", results[0].ErrorMessage);
        }

        [TestMethod]
        public void ItShowsMultipleErrorsIfRequired()
        {
            NewProductResource input = new NewProductResource
            {
                CategoryIds = new List<int>{
                    1,2,3,3
                }
            };

            var context = new ValidationContext(input, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(input, context, results, true);

            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(4, results.Count);
            Assert.AreEqual("A product name must be provided", results[0].ErrorMessage);
            Assert.AreEqual("A description of the product must be provided", results[1].ErrorMessage);
            Assert.AreEqual("Price must be between 0.01 and 9999.99", results[2].ErrorMessage);
            Assert.AreEqual("a categoryId can't be used multiple times", results[3].ErrorMessage);
        }

        [TestMethod]
        public void UniqueEntriesInListReturnsTrueIfCorrectt()
        {
            List<int> ints = new List<int>
            {
                1,2,3
            };

            UniqueEntriesInListAttribute attribute = new UniqueEntriesInListAttribute();

            bool result = attribute.IsValid(ints);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UniqueEntriesInListReturnsFalseIfNot()
        {
            List<int> ints = new List<int>
            {
                1,2,3,3
            };

            UniqueEntriesInListAttribute attribute = new UniqueEntriesInListAttribute();

            bool result = attribute.IsValid(ints);

            Assert.IsFalse(result);
        }
    }
}

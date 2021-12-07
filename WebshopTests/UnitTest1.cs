using System.Collections.Generic;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using S3_webshop.Controllers;
using S3_webshop;
using System.Linq;

namespace WebshopTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int id = 20;

            Assert.AreEqual(20, id);
        }

        [TestMethod]
        public void TestMethod2()
        {
            int id = 20;

            Assert.AreEqual(25, id+5);
        }

        [TestMethod]
        public void TestMethod3()
        {
            int id = 20;

            Assert.AreNotEqual(25, id);
        }

        [TestMethod]
        public void TestMethod4()
        {
            WeatherForecastController controller = new WeatherForecastController();

            int result = controller.Testfunction(10);
            Assert.AreEqual(20, result);
        }

        [TestMethod]
        public void TestWeatherControllerGet()
        {
            WeatherForecastController controller = new WeatherForecastController();

            List<WeatherForecast> result = controller.Get().ToList();

            Assert.AreEqual(5, result.Count);
        }
    }
}

using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using System;
using src.Controllers;
using Microsoft.Extensions.Configuration;

namespace src.unitTests
{
    public class ShopProductTests
    {
        private ShopController shop;

        [SetUp]
        public void SetUp(IConfiguration configuration)
        {
            shop = new ShopController(configuration);
        }

        // MySQL column is labeled as String
        [TestCase("31")]
        [TestCase("-1")]
        [Test]
        public void ProductPage_GivenNegativeOrInvalidID_ReturnsError(string id)
        {
            // Arrange
            var WebShop = new List<Models.ShopModel>
            {
                new Models.ShopModel {Id = "32", Name = "7Up", Description = "A beverage", Price = 24.5, Amount = 0}
            };

            // Act
            var result = this.shop.product(id);

            // Assert
            Assert.That(result, Is.EqualTo("There is no such product."));

        }
    }
}

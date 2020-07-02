using AutoFixture;
using System;
using exchange.Services;
using Xunit;
using exchange.Controllers;
using FakeItEasy;
using exchange.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contracts;

namespace exchange.Tests
{
    public class PurchaseControllerTest
    {
        //Fakes
        private readonly PurchaseService _purchaseService;
        private readonly ExchangeController _exchangeController;
        private readonly ILoggerManager _logger;

        //Dummy Data Generator
        private readonly Fixture _fixture;

        //System under test
        private readonly PurchasesController _sut;
        public PurchaseControllerTest()
        {
            _exchangeController = A.Fake<ExchangeController>();
            _purchaseService = A.Fake<PurchaseService>();
            _logger = A.Fake<ILoggerManager>();
            _sut = new PurchasesController(_purchaseService, _exchangeController, _logger);

            _fixture = new Fixture();
        }

        [Fact]
        public void Get_WhereThereArePurchases_ShouldReturnActionResultOfPurcharsesWith200StatusCode()
        {
            //Arrange
            var products = _fixture.CreateMany<Purchase>(3).ToList();
            A.CallTo(() => _purchaseService.GetByUserIdAndMonthAndCurrency(products.First())).Returns(products);

            //Act
            var result = _sut.Get(products.First().Id) as OkObjectResult;

            //Assert
            A.CallTo(() => _purchaseService.GetByUserIdAndMonthAndCurrency(products.First())).MustHaveHappenedOnceExactly();
            Assert.NotNull(result);
            var returnValue = Assert.IsType<List<Purchase>>(result.Value);
            Assert.Equal(products.Count, returnValue.Count());
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}

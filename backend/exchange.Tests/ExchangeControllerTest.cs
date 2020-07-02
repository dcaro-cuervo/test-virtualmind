using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using exchange.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace exchange.Tests
{
    public class ExchangeControllerTest
    {
        //Fakes

        //Dummy Data Generator
        private readonly Fixture _fixture;

        //System under test
        private readonly ExchangeController _sut;
        public ExchangeControllerTest()
        {
            _sut = new ExchangeController();

            _fixture = new Fixture();
        }

        [Fact]
        public async Task Get_Dolar_ExchangeRate_ShouldReturnActionResultOfExchangeWith200StatusCode()
        {
            //Arrange
            const string currency = "dolar";

            //Act
            var result = await _sut.ExchangeRate(currency) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task Get_Real_ExchangeRate_ShouldReturnActionResultOfExchangeWith200StatusCode()
        {
            //Arrange
            const string currency = "real";

            //Act
            var result = await _sut.ExchangeRate(currency) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task Get_Canadian_Dolar_ExchangeRate_ShouldReturnActionResultOfExchangeWith500StatusCode()
        {
            //Arrange
            const string currency = "canadian";

            // Act
            var result = await _sut.ExchangeRate(currency) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task Get_WhenThereIsUnhandledException_ShouldReturn500StatusCode()
        {

        }
    }
}

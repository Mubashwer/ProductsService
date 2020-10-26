using Microsoft.AspNetCore.Mvc;
using Products.API.Controllers;
using Xunit;

namespace Products.API.UnitTests.Tests.Controllers
{
    public sealed class DefaultControllerTests
    {
        private readonly DefaultController _sut;

        public DefaultControllerTests()
        {
            _sut = new DefaultController();
        }

        [Fact]
        public void Index_Always_RedirectsToSwaggerUI()
        {
            //Act
            var result = _sut.Index();

            //Assert
            Assert.IsType<RedirectResult>(result);
            Assert.Equal("~/swagger", ((RedirectResult)result).Url);
        }

        [Fact]
        public void Index_Always_ReturnsOk()
        {
            //Act
            var result = _sut.Ping();

            //Assert
            Assert.IsType<OkResult>(result);
        }
    }
}

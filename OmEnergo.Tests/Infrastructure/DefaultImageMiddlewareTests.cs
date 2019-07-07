using Microsoft.AspNetCore.Http;
using Moq;
using OmEnergo.Infrastructure;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace OmEnergo.Tests.Infrastructure
{
    public class DefaultImageMiddlewareTests
    {
        private Task DelegateWhichDoesNothing(HttpContext httpContext) => Task.Run(() => { });

        [Fact]
        public async Task DefaultImageReturned()
        {
            //Arrange
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request.Headers["accept"]).Returns("image/webp,image/apng,image/*,*/*;q=0.8");
            httpContextMock.Setup(x => x.Response.StatusCode).Returns(404);
            var httpContext = httpContextMock.Object;
            RequestDelegate requestDelegate = DelegateWhichDoesNothing;
            var defaultImageMiddleware = new DefaultImageMiddleware(requestDelegate);

            //Act

            //Assert
            //The DirectoryNotFoundException is thrown only in case, if the middleware tries to get the default image,
            //which does not exist in the current Tests project folder 
            await Assert.ThrowsAsync<DirectoryNotFoundException>(() => defaultImageMiddleware.Invoke(httpContext));
        }

        [Fact]
        public async Task DefaultImageIsNotReturned_AnotherAcceptHeader()
        {
            //Arrange
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request.Headers["accept"]).Returns("*/*");
            httpContextMock.Setup(x => x.Response.StatusCode).Returns(404);
            var httpContext = httpContextMock.Object;
            RequestDelegate requestDelegate = DelegateWhichDoesNothing;
            var defaultImageMiddleware = new DefaultImageMiddleware(requestDelegate);

            //Act
            await defaultImageMiddleware.Invoke(httpContext);

            //Assert
            Assert.Null(httpContext.Response.Body);
        }

        [Fact]
        public async Task DefaultImageIsNotReturned_AnotherStatusCode()
        {
            //Arrange
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request.Headers["accept"]).Returns("image/webp,image/apng,image/*,*/*;q=0.8");
            httpContextMock.Setup(x => x.Response.StatusCode).Returns(200);
            var httpContext = httpContextMock.Object;
            RequestDelegate requestDelegate = DelegateWhichDoesNothing;
            var defaultImageMiddleware = new DefaultImageMiddleware(requestDelegate);

            //Act
            await defaultImageMiddleware.Invoke(httpContext);

            //Assert
            Assert.Null(httpContext.Response.Body);
        }
    }
}

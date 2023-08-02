using FerrumCapital.Application.Common.Interfaces;
using FerrumCapital.Application.Features.Commands.Product.CreateProduct;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Products
{
    public class CreateProductTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ShouldCreateProductAndSaveFile()
        {
            // Arrange
            var mockDbContext = new Mock<IApplicationDbContext>();
            var request = new CreateProductCommandRequest(GetMockFormFile("test.jpg", "image/jpeg", 1024)); // 1024 bytes, valid size
            var handler = new CreateProductCommandHandler(mockDbContext.Object);

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.IsSuccess);
        }

        [Theory]
        [InlineData("test.txt", "text/plain")] // Invalid MIME type
        [InlineData("test.jpg", "image/jpeg", 20 * 1024 * 1024)] // File size exceeds the limit
        [InlineData("test.gif", "image/gif")] // Invalid file extension
        public async Task Handle_InvalidRequest_ShouldReturnErrorResponse(string fileName, string contentType, long fileSize = 1024)
        {
            // Arrange
            var mockDbContext = new Mock<IApplicationDbContext>();
            var request = new CreateProductCommandRequest(GetMockFormFile(fileName, contentType, fileSize));
            var handler = new CreateProductCommandHandler(mockDbContext.Object);

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.IsSuccess);
        }

        private IFormFile GetMockFormFile(string fileName, string contentType, long fileSize)
        {
            var content = new byte[fileSize];
            var stream = new MemoryStream(content);
            return new FormFile(stream, 0, fileSize, "Data", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }
}

using FerrumCapital.Application.Common.Interfaces;
using FerrumCapital.Application.Features.Commands.Product.DeleteProduct;
using FerrumCapital.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Products
{
    public class DeleteProductTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ShouldDeleteProduct()
        {
            // Arrange
            var productIdToDelete = 1;
            var mockDbContext = new Mock<IApplicationDbContext>();
            var handler = new DeleteProductCommandHandler(mockDbContext.Object);
            var productToDelete = new Product { Id = productIdToDelete, Name = "Test Product" };

            // Mock the DbContext to return the product to be deleted
            mockDbContext.Setup(c => c.Products.FindAsync(productIdToDelete)).ReturnsAsync(productToDelete);

            // Act
            var request = new DeleteProductCommandRequest { Id = productIdToDelete };
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.IsSuccess);
            // Verify that the product was removed from the context.
            mockDbContext.Verify(c => c.Products.Remove(productToDelete), Times.Once);
            // Verify that the SaveChangesAsync method was called once.
            mockDbContext.Verify(c => c.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidRequest_ShouldReturnErrorResponse()
        {
            // Arrange
            var invalidProductId = -1; // A non-existing product ID
            var mockDbContext = new Mock<IApplicationDbContext>();
            var handler = new DeleteProductCommandHandler(mockDbContext.Object);

            // Act
            var request = new DeleteProductCommandRequest { Id = invalidProductId };
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.IsSuccess);
            // Assert that the product with the given invalid ID was not removed from the context.
            mockDbContext.Verify(c => c.Products.Remove(It.IsAny<Product>()), Times.Never);
            // Assert that SaveChangesAsync method was not called.
            mockDbContext.Verify(c => c.SaveChangesAsync(CancellationToken.None), Times.Never);
        }
    }
}

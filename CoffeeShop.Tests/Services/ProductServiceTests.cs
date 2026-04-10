using System;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using WebApp_Coffe.DTOs;
using WebApp_Coffe.Models;
using WebApp_Coffe.Repositories;
using WebApp_Coffe.Services;
using Xunit;

namespace CoffeeShop.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly Mock<IFileUploadService> _mockUploadService;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _mockRepo = new Mock<IProductRepository>();
        _mockUploadService = new Mock<IFileUploadService>();
        _service = new ProductService(_mockRepo.Object, _mockUploadService.Object);
    }

    [Fact]
    public async Task GetProductsAsync_NoFilters_ShouldReturnOnlyActiveProducts()
    {
        // Arrange
        var mockData = MockData.GetProducts().BuildMock();
        _mockRepo.Setup(r => r.Query()).Returns(mockData);

        // Act
        var response = await _service.GetProductsAsync(null, null, null, null, 1, 10);

        // Assert
        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.Items.Should().HaveCount(2); // Espresso and Latte are active
        response.Data.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task GetProductsAsync_WithFilters_ShouldReturnFilteredProducts()
    {
        // Arrange
        var mockData = MockData.GetProducts().BuildMock();
        _mockRepo.Setup(r => r.Query()).Returns(mockData);

        // Act (Filter by tag = bestseller)
        var response = await _service.GetProductsAsync(null, "bestseller", null, null, 1, 10);

        // Assert
        response.Success.Should().BeTrue();
        response.Data!.Items.Should().HaveCount(1);
        response.Data.Items.First().Name.Should().Be("Espresso");
    }

    [Fact]
    public async Task GetBySlugAsync_WhenExists_ShouldReturnProduct()
    {
        // Arrange
        var product = MockData.GetProducts().First(p => p.IsActive);
        _mockRepo.Setup(r => r.GetBySlugAsync("espresso")).ReturnsAsync(product);

        // Act
        var response = await _service.GetBySlugAsync("espresso");

        // Assert
        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.Slug.Should().Be("espresso");
    }

    [Fact]
    public async Task GetBySlugAsync_WhenNotExists_ShouldReturnFail()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetBySlugAsync("invalid")).ReturnsAsync((Product?)null);

        // Act
        var response = await _service.GetBySlugAsync("invalid");

        // Assert
        response.Success.Should().BeFalse();
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WithFile_ShouldUploadAndReturnSuccess()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        _mockUploadService.Setup(u => u.UploadAsync(mockFile.Object, "products"))
            .ReturnsAsync("/uploads/products/test.jpg");

        var request = new ProductRequest
        {
            Name = "New Coffee",
            CategoryId = Guid.NewGuid(),
            ImageFile = mockFile.Object
        };

        _mockRepo.Setup(r => r.AddAsync(It.IsAny<Product>())).ReturnsAsync(new Product());

        // Act
        var response = await _service.CreateAsync(request);

        // Assert
        response.Success.Should().BeTrue();
        _mockUploadService.Verify(u => u.UploadAsync(mockFile.Object, "products"), Times.Once);
        _mockRepo.Verify(r => r.AddAsync(It.Is<Product>(p => p.ImageUrl == "/uploads/products/test.jpg")), Times.Once);
        _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenExists_ShouldUpdateFieldsAndUploadFile()
    {
        // Arrange
        var productId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1");
        var existingProduct = MockData.GetProducts().First(p => p.Id == productId);
        var mockData = new List<Product> { existingProduct }.BuildMock();
        
        _mockRepo.Setup(r => r.Query()).Returns(mockData);

        var mockFile = new Mock<IFormFile>();
        _mockUploadService.Setup(u => u.UploadAsync(mockFile.Object, "products"))
            .ReturnsAsync("/uploads/products/updated.jpg");

        var request = new ProductRequest
        {
            Name = "Updated Coffee",
            CategoryId = Guid.NewGuid(),
            ImageFile = mockFile.Object
        };

        // Act
        var response = await _service.UpdateAsync(productId, request);

        // Assert
        response.Success.Should().BeTrue();
        existingProduct.Name.Should().Be("Updated Coffee");
        existingProduct.ImageUrl.Should().Be("/uploads/products/updated.jpg");
        _mockRepo.Verify(r => r.UpdateAsync(existingProduct), Times.Once);
        _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenExists_ShouldReturnSuccess()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product { Id = productId };
        _mockRepo.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

        // Act
        var response = await _service.DeleteAsync(productId);

        // Assert
        response.Success.Should().BeTrue();
        _mockRepo.Verify(r => r.DeleteAsync(product), Times.Once);
        _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}

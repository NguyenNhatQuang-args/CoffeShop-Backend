using System;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Tests.Helpers;
using FluentAssertions;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using WebApp_Coffe.DTOs;
using WebApp_Coffe.Models;
using WebApp_Coffe.Repositories;
using WebApp_Coffe.Services;
using Xunit;

namespace CoffeeShop.Tests.Services;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _mockRepo;
    private readonly Mock<IFileUploadService> _mockUploadService;
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _mockRepo = new Mock<ICategoryRepository>();
        _mockUploadService = new Mock<IFileUploadService>();
        _service = new CategoryService(_mockRepo.Object, _mockUploadService.Object);
    }

    [Fact]
    public async Task GetAllActiveAsync_ShouldReturnOnlyActiveCategories()
    {
        // Arrange
        var mockData = MockData.GetCategories().BuildMock();
        _mockRepo.Setup(r => r.Query()).Returns(mockData);

        // Act
        var response = await _service.GetAllActiveAsync();

        // Assert
        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data.Should().HaveCount(1);
        response.Data!.First().Name.Should().Be("Coffee");
    }

    [Fact]
    public async Task GetBySlugAsync_WhenExists_ShouldReturnCategoryWithProducts()
    {
        // Arrange
        var category = MockData.GetCategories().First(c => c.IsActive);
        _mockRepo.Setup(r => r.GetBySlugAsync("coffee")).ReturnsAsync(category);

        // Act
        var response = await _service.GetBySlugAsync("coffee");

        // Assert
        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.Slug.Should().Be("coffee");
    }

    [Fact]
    public async Task GetBySlugAsync_WhenNotExists_ShouldReturnFail()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetBySlugAsync("invalid")).ReturnsAsync((Category?)null);

        // Act
        var response = await _service.GetBySlugAsync("invalid");

        // Assert
        response.Success.Should().BeFalse();
        response.Message.Should().Contain("not found");
        response.Data.Should().BeNull();
    }
}

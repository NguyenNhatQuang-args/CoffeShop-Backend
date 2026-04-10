using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using WebApp_Coffe.Services;
using Xunit;

namespace CoffeeShop.Tests.Services;

public class FileUploadServiceTests : IDisposable
{
    private readonly Mock<IWebHostEnvironment> _mockEnv;
    private readonly FileUploadService _service;
    private readonly string _tempWebRoot;

    public FileUploadServiceTests()
    {
        _tempWebRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempWebRoot);

        _mockEnv = new Mock<IWebHostEnvironment>();
        _mockEnv.Setup(e => e.WebRootPath).Returns(_tempWebRoot);

        _service = new FileUploadService(_mockEnv.Object);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempWebRoot))
        {
            Directory.Delete(_tempWebRoot, true);
        }
    }

    [Fact]
    public async Task UploadAsync_ValidFile_ShouldReturnUrlAndCreateFile()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        var content = "dummy image content";
        var fileName = "test.jpg";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;

        mockFile.Setup(f => f.FileName).Returns(fileName);
        mockFile.Setup(f => f.Length).Returns(ms.Length);
        mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream, token));

        // Act
        var result = await _service.UploadAsync(mockFile.Object, "products");

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().StartWith("/uploads/products/");
        result.Should().EndWith(".jpg");

        var filePath = Path.Combine(_tempWebRoot, result.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        File.Exists(filePath).Should().BeTrue();
    }

    [Fact]
    public async Task UploadAsync_EmptyFile_ShouldThrowArgumentException()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(0);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.UploadAsync(mockFile.Object, "products"));
    }

    [Fact]
    public async Task UploadAsync_LargeFile_ShouldThrowArgumentException()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(6 * 1024 * 1024); // 6MB

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.UploadAsync(mockFile.Object, "products"));
        ex.Message.Should().Contain("5MB");
    }

    [Fact]
    public async Task UploadAsync_InvalidExtension_ShouldThrowArgumentException()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(1024);
        mockFile.Setup(f => f.FileName).Returns("document.pdf");

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.UploadAsync(mockFile.Object, "products"));
        ex.Message.Should().Contain("Invalid file extension");
    }
}

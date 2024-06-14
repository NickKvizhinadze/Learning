using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Dometrain.EFCore.SimpleAPI.Data;
using Dometrain.EFCore.SimpleAPI.Models;
using Dometrain.EFCore.SimpleAPI.Controllers;
using MockQueryable.NSubstitute;
using Dometrain.EfCore.SimpleAPI.Repositories;

namespace Dometrain.EFCore.Tests.MemoryDatabase;

public class RepositoryTest
{
    [Fact]
    public async Task IfGenreExists_ReturnsGenre()
    {
        var repository = Substitute.For<IGenreRepository>();
        repository.Get(2).Returns(new Genre { Id = 2, Name = "Action" });
        
        // Arrange
        var controller = new GenresWithRepositoryController(repository);

        // Act
        var response = await controller.Get(2);
        var okResult = response as OkObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal("Action", (okResult.Value as Genre)?.Name);

        await repository.Received().Get(2);
    }
}
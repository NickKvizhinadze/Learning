using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using RiverBooks.Books.BookEndpoints;
using RiverBooks.Books.Models;
using RiverBooks.Books.Tests.Endpoints;
using Xunit.Abstractions;

namespace RiverBooks.Books.Tests.Endpoints;

public class BookGetById(Fixture fixture, ITestOutputHelper outputHelper)
    : TestBase<Fixture>()
{
    [Theory]
    [InlineData("476d9cf4-4c88-4341-9fbe-eef8703e40e7", "The Fellowship of the Ring")]
    [InlineData("1f41c855-1300-404f-8735-47fae437966d", "The Two Towers")]
    [InlineData("5f23f2df-434c-4383-aae7-04652678d4dc", "The Return of the King")]
    public async Task ReturnsExpectedBookGivenId(string id, string title)
    {
        var request = new GetBookByIdRequest { Id = Guid.Parse(id) };
        var testResult = await fixture.Client.GETAsync<GetById, GetBookByIdRequest, BookDto>(request);

        testResult.Response.EnsureSuccessStatusCode();

        testResult.Result!.Title.Should().Be(title);
    }
}
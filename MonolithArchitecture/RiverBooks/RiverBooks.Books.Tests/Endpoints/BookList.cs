using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Xunit.Abstractions;
using RiverBooks.Books.Tests.Endpoints;
using RiverBooks.Books.BookEndpoints;
using RiverBooks.Books.Data.Configurations;

namespace RiverBooks.Books.Tests.Endpoints;

public class BookList(Fixture fixture, ITestOutputHelper outputHelper)
    : TestBase<Fixture>()
{
    [Fact]
    public async Task ReturnsTreeBooksAsync()
    {
        var testResult = await fixture.Client.GETAsync<List, ListBooksResponse>();

        testResult.Response.EnsureSuccessStatusCode();

        testResult.Result.Books!.Count.Should().Be(3);
    }
}
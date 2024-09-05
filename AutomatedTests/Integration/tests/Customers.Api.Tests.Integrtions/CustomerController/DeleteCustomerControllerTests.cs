using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;

namespace Customers.Api.Tests.Integrtions.CustomerController;

public class DeleteCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly CustomerApiFactory _apiFactory;
    private readonly HttpClient _client;

    private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date)
        .RuleFor(x => x.GitHubUsername, _ => CustomerApiFactory.ValidGithubUser);


    public DeleteCustomerControllerTests(CustomerApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenCustomerExists()
    {
        var customerId = await CreateCostumer();
        // Act
        var response = await _client.DeleteAsync($"customers/{customerId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        response = await _client.GetAsync($"customers/{customerId}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task Delete_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        var customerId = Guid.NewGuid();
        // Act
        var response = await _client.DeleteAsync($"customers/{customerId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    
    private async Task<Guid> CreateCostumer()
    {
        var customer = _customerGenerator.Generate();
        var response = await _client.PostAsJsonAsync("customers", customer);
        var content = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        return content!.Id;
    }
}
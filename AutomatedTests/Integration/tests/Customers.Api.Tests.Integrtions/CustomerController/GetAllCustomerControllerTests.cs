using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Tests.Integrtions.CustomerController;

public class GetAllCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly CustomerApiFactory _apiFactory;
    private readonly HttpClient _client;

    private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date)
        .RuleFor(x => x.GitHubUsername, _ => CustomerApiFactory.ValidGithubUser);


    public GetAllCustomerControllerTests(CustomerApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsAllCustomers_WhenCustomersExist()
    {
        var range = Enumerable.Range(0, 3).ToList();
        foreach (var _ in range)
            await CreateCostumer();

        // Act
        var response = await _client.GetAsync($"customers");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var customerResponse = await response.Content.ReadFromJsonAsync<GetAllCustomersResponse>();

        customerResponse.Should().NotBeNull();
        customerResponse!.Customers.Count().Should().Be(range.Count);
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyResult_WhenNoCustomersExist()
    {
        // Act
        var response = await _client.GetAsync($"customers");

        // Assert
        var customerResponse = await response.Content.ReadFromJsonAsync<GetAllCustomersResponse>();

        customerResponse.Should().NotBeNull();
        customerResponse!.Customers.Count().Should().Be(0);
    }


    private async Task<Guid> CreateCostumer()
    {
        var customer = _customerGenerator.Generate();
        var response = await _client.PostAsJsonAsync("customers", customer);
        var content = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        return content!.Id;
    }
}
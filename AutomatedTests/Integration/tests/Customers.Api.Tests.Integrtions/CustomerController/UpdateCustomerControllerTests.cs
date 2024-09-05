using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Tests.Integrtions.CustomerController;

public class UpdateCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly CustomerApiFactory _apiFactory;
    private readonly HttpClient _client;

    private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date)
        .RuleFor(x => x.GitHubUsername, _ => CustomerApiFactory.ValidGithubUser);


    public UpdateCustomerControllerTests(CustomerApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Update_UpdatesUser_WhenDataIsValid()
    {
        var customerId = await CreateCostumer();

        var customer = _customerGenerator.Generate();
        
        // Act
        var response = await _client.PutAsJsonAsync($"customers/{customerId}", customer);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>();

        customerResponse.Should().NotBeNull();
        customerResponse.Should().BeEquivalentTo(customer);
    }
    
    [Fact]
    public async Task Create_ReturnsValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        var customerId = await CreateCostumer();

        const string invalidEmail = "tasjbnfhjsn";
        var customer = _customerGenerator.Clone()
            .RuleFor(x => x.Email, invalidEmail)
            .Generate();

        // Act
        var response = await _client.PutAsJsonAsync($"customers/{customerId}", customer);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");

        error.Errors["Email"][0].Should().Be($"{invalidEmail} is not a valid email address");
    }
    
    [Fact]
    public async Task Create_ReturnsValidationError_WhenGitHubUserDoesNotExists()
    {
        // Arrange
        var customerId = await CreateCostumer();

        const string invalidGithubUser = "tasjbnfhjsn";
        var customer = _customerGenerator.Clone()
            .RuleFor(x => x.GitHubUsername, invalidGithubUser)
            .Generate();

        // Act
        var response = await _client.PutAsJsonAsync($"customers/{customerId}", customer);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");

        error.Errors["Customer"][0].Should().Be($"There is no GitHub user with username {invalidGithubUser}");
    }
    
    
    private async Task<Guid> CreateCostumer()
    {
        var customer = _customerGenerator.Generate();
        var response = await _client.PostAsJsonAsync("customers", customer);
        var content = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        return content!.Id;
    }
}
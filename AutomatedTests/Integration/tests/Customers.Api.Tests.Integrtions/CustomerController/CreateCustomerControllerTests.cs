using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Tests.Integrtions.CustomerController;

public class CreateCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly CustomerApiFactory _apiFactory;
    private readonly HttpClient _client;

    private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date)
        .RuleFor(x => x.GitHubUsername, _ => CustomerApiFactory.ValidGithubUser);


    public CreateCustomerControllerTests(CustomerApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Create_CreateUser_WhenDataIsValid()
    {
        // Arrange
        var customer = _customerGenerator.Generate();

        // Act
        var response = await _client.PostAsJsonAsync("customers", customer);

        // Assert
        var customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        customerResponse.Should().BeEquivalentTo(customer);
        response.Headers.Location!.ToString().Should()
            .Be($"http://localhost/customers/{customerResponse!.Id}");
    }
    
    [Fact]
    public async Task Create_ReturnsValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        const string invalidEmail = "tasjbnfhjsn";
        var customer = _customerGenerator.Clone()
            .RuleFor(x => x.Email, invalidEmail);

        // Act
        var response = await _client.PostAsJsonAsync("customers", customer);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");

        error.Errors["Email"][0].Should().Be("'Email' must not be empty.");
    }
    
    [Fact]
    public async Task Create_ReturnsValidationError_WhenGitHubUserDoesNotExists()
    {
        // Arrange
        const string invalidGithubUser = "tasjbnfhjsn";
        var customer = _customerGenerator.Clone()
            .RuleFor(x => x.GitHubUsername, invalidGithubUser)
            .Generate();

        // Act
        var response = await _client.PostAsJsonAsync("customers", customer);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");

        error.Errors["Customer"][0].Should().Be($"There is no GitHub user with username {invalidGithubUser}");
    }
    
    [Fact]
    public async Task Create_ReturnsInternalServerError_WhenGitHubUserThrottled()
    {
        // Arrange
        const string invalidGithubUser = "tasjbnfhjsn";
        var customer = _customerGenerator.Clone()
            .RuleFor(x => x.GitHubUsername, CustomerApiFactory.ThrottledUser)
            .Generate();

        // Act
        var response = await _client.PostAsJsonAsync("customers", customer);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
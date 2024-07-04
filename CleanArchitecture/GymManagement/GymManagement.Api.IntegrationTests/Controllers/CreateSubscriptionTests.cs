using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GymManagement.Api.IntegrationTests.Common;
using GymManagement.Contracts.Subscriptions;
using GymManagement.TestCommon.TestConstants;

namespace GymManagement.Api.IntegrationTests.Controllers;

[Collection(GymManagementApiFactoryCollection.CollectionName)]
public class CreateSubscriptionTests
{
    private readonly HttpClient _httpClient;

    public CreateSubscriptionTests(GymManagementApiFactory apiFactory)
    {
        _httpClient = apiFactory.HttpClient;
        apiFactory.ResetDatabase();
    }

    [Theory]
    [MemberData(nameof(ListSubscriptionTypes))]
    public async Task CreateSubscription_WhenValidSubscription_ShouldCreateSubscription(SubscriptionType subscriptionType)
    {
        var createSubscriptionRequest = new CreateSubscriptionRequest(subscriptionType, Constants.Admin.Id);
        var response = await _httpClient.PostAsJsonAsync("Subscriptions", createSubscriptionRequest);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        
        var subscription = await response.Content.ReadFromJsonAsync<SubscriptionResponse>();
        subscription.Should().NotBeNull();
        subscription!.SubscriptionType.Should().Be(subscriptionType);
        
        response.Headers.Location!.ToString().Should().Contain($"/Subscriptions/{subscription.Id}");
    }
    
    

    public static TheoryData<SubscriptionType> ListSubscriptionTypes()
    {
        var subscriptionTypes = Enum.GetValues<SubscriptionType>().ToList();

        var theoryData = new TheoryData<SubscriptionType>();

        foreach (var subscriptionType in subscriptionTypes)
        {
            theoryData.Add(subscriptionType);
        }

        return theoryData;
    }
}
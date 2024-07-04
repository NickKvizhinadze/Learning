using ErrorOr;
using FluentAssertions;
using GymManagement.Application.SubcutaneousTests.Common;
using GymManagement.Domain.Subscriptions;
using GymManagement.TestCommon.Gyms;
using GymManagement.TestCommon.Subscriptions;
using MediatR;

namespace GymManagement.Application.SubcutaneousTests.Gyms.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class CreateGymTests
{
    private readonly IMediator _mediator;

    public CreateGymTests(MediatorFactory mediatorFactory)
    {
        _mediator = mediatorFactory.CreateMediator();
    }
    
    [Fact]
    public async Task CreateGym_WhenValidCommand_ShouldCreateGym()
    {
        var subscription = await CreateSubscription();

        var createGymCommand = GymCommandFactory.CreateCreateGymCommand(subscriptionId: subscription.Id);
        
        var result = await _mediator.Send(createGymCommand);
        
        result.IsError.Should().BeFalse();
        var gym = result.Value;
        gym.SubscriptionId.Should().Be(subscription.Id);
    }

    private async Task<Subscription> CreateSubscription()
    {
        var createSubscriptionCommand = SubscriptionCommandFactory.CreateSubscriptionCommandFactory();

        var result = await _mediator.Send(createSubscriptionCommand);

        result.IsError.Should().BeFalse();
        return result.Value;
    }
}
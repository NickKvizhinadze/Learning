using MediatR;
using Microsoft.AspNetCore.Mvc;
using GymManagement.Contracts.Subscriptions;
using GymManagement.Application.Subscriptions.Queries.GetSubscription;
using GymManagement.Application.Subscriptions.Command.CreateSubscription;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController(ISender mediator) : ControllerBase
{
    [HttpGet("{subscriptionId:guid}")]
    public async Task<IActionResult> GetSubscription(Guid subscriptionId)
    {
        var query = new GetSubscriptionQuery(subscriptionId);
        var subscriptionResult = await mediator.Send(query);

        return subscriptionResult.MatchFirst(
            subscription => Ok(new SubscriptionResponse(subscription.Id,
                    Enum.Parse<SubscriptionType>(subscription.SubscriptionType))),
                error => Problem()
            );
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionRequest request)
    {
        var command = new CreateSubscriptionCommand(request.SubscriptionType.ToString(), request.AdminId);
        var createSubscriptionResult = await mediator.Send(command);

        return createSubscriptionResult.MatchFirst(
            subscription => Ok(new CreateSubscriptionResponse(subscription.Id, request.SubscriptionType)), 
            error => Problem());
    }
}
using GymManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;
using GymManagement.Contracts.Subscriptions;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController(ISubscriptionsService subscriptionsService) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateSubscription([FromBody] CreateSubscriptionRequest request)
    {
        var subscriptionId =
            subscriptionsService.CreateSubscription(request.SubscriptionType.ToString(), request.AdminId);

        var response = new CreateSubscriptionResponse(subscriptionId, request.SubscriptionType);

        return Ok(response);
    }
}
using MediatR;
using ErrorOr;
using GymManagement.Domain.Subscriptions;

namespace GymManagement.Application.Subscriptions.Command.CreateSubscription;

public record CreateSubscriptionCommand(SubscriptionType SubscriptionType, Guid AdminId) : IRequest<ErrorOr<Subscription>>;
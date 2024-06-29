using GymManagement.Domain.Subscriptions;
using MediatR;
using ErrorOr;

namespace GymManagement.Application.Subscriptions.Queries.GetSubscription;

public record GetSubscriptionQuery(Guid SubscriptionId) : IRequest<ErrorOr<Subscription>>;
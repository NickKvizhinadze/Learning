namespace GymManagement.Contracts.Subscriptions;

public record CreateSubscriptionRequest(SubscriptionType Type, Guid AdminId);
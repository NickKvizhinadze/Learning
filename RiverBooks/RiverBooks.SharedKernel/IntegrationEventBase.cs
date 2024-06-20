using MediatR;

namespace RiverBooks.SharedKernel;

public abstract record IntegrationEventBase : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
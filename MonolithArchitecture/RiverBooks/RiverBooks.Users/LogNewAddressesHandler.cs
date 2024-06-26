using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.Users.Events;

namespace RiverBooks.Users;

internal class LogNewAddressesHandler(ILogger<LogNewAddressesHandler> logger): INotificationHandler<AddressAddedEvent>
{
    public Task Handle(AddressAddedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("[DE Handler] New address added to user {user}: {address}",
            notification.NewAddress.UserId,
            notification.NewAddress.StreetAddress);

        return Task.CompletedTask;
    }
}
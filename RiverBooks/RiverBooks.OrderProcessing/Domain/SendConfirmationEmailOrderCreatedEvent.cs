using MediatR;
using RiverBooks.EmailSending.Contracts;
using RiverBooks.Users.Contracts;

namespace RiverBooks.OrderProcessing.Domain;

internal class SendConfirmationEmailOrderCreatedEvent(IMediator mediator): INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        var getUserByIdQuery = new UserDetailsByIdQuery(notification.Order.UserId.ToString());
        var userResult = await mediator.Send(getUserByIdQuery);
        if(!userResult.IsSuccess)
            return;
        
        var sendEmailCommand = new SendEmailCommand
        {
            To = userResult.Value.Email,
            From = "notreply@test.com",
            Subject = "Your RiverBooks purchase",
            Body = $"Tou bought {notification.Order.OrderItems.Count()} items"
        };
        
        await mediator.Send(sendEmailCommand);
        
    }
}
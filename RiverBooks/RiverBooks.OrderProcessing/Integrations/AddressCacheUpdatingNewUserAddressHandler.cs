using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.OrderProcessing.Domain;
using RiverBooks.OrderProcessing.Infrastructure;
using RiverBooks.OrderProcessing.Interfaces;
using RiverBooks.OrderProcessing.Models;
using RiverBooks.Users.Contracts;

namespace RiverBooks.OrderProcessing.Integrations;

internal class AddressCacheUpdatingNewUserAddressHandler(
    IOrderAddressCache addressCache,
    ILogger<AddressCacheUpdatingNewUserAddressHandler> logger) :
    INotificationHandler<NewUserAddressAddedIntegrationEvent>
{
    public async Task Handle(NewUserAddressAddedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var addressDetails = notification.AddressDetails;
        var orderAddress = new OrderAddress(addressDetails.AddressId,
            new Address(
                addressDetails.Street1,
                addressDetails.Street2,
                addressDetails.City,
                addressDetails.State,
                addressDetails.PostalCode,
                addressDetails.Country
            ));

        await addressCache.StoreAsync(orderAddress);
        
        logger.LogInformation("Cached updated with new address {address}", orderAddress);
    }
}
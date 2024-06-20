using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.UseCases.User;

internal class AddAddressToUserHandler(
    IApplicationUserRepository userRepository,
    ILogger<AddAddressToUserHandler> logger)
    : IRequestHandler<AddAddressToUserCommand, Result>
{
    public async Task<Result> Handle(AddAddressToUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.EmailAddress))
            return Result.Unauthorized();
        
        var user = await userRepository.GetUserWithAddressesByEmailAsync(request.EmailAddress);
        if (user is null)
            return Result.Unauthorized();

        var address = new Address(
            request.Street1,
            request.Street2,
            request.City,
            request.State,
            request.PostalCode,
            request.Country
        );
        
        var userAddress = user.AddAddress(address);

        await userRepository.SaveChangesAsync();
        
        logger.LogInformation("[UseCase] Added address {address} to user {EmailAddress} (Total: {total}", 
            userAddress.StreetAddress,
            request.EmailAddress,
            user.Addresses.Count);

        return Result.Success();
    }
}
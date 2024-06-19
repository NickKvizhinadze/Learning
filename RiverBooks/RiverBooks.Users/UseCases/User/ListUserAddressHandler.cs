using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.Users.Entities;
using RiverBooks.Users.Models;
using RiverBooks.Users.Repositories;

namespace RiverBooks.Users.UseCases.User;

internal class ListUserAddressHandler(
    IApplicationUserRepository userRepository,
    ILogger<AddAddressToUserHandler> logger)
    : IRequestHandler<ListUserAddressQuery, Result<List<UserAddressDto>>>
{
    public async Task<Result<List<UserAddressDto>>> Handle(ListUserAddressQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.EmailAddress))
            return Result.Unauthorized();

        var user = await userRepository.GetUserWithAddressesByEmailAsync(request.EmailAddress);
        if (user is null)
            return Result.Unauthorized();
        
        var userAddresses = user.Addresses
            .Select(a => new UserAddressDto(
                user.Id,
                a.StreetAddress.Street1,
                a.StreetAddress.Street2,
                a.StreetAddress.City,
                a.StreetAddress.State,
                a.StreetAddress.PostalCode,
                a.StreetAddress.Country
                ))
            .ToList();


        return Result.Success(userAddresses);
    }
}
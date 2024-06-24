using RiverBooks.Users.Models;

namespace RiverBooks.Users.UsersEndpoints.ListAddress;

public class ListAddressResponse
{
    public ListAddressResponse(List<UserAddressDto> addresses)
    {
        Addresses = addresses;
    }

    public List<UserAddressDto> Addresses { get; set; }
};
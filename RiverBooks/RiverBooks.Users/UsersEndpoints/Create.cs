using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using RiverBooks.Users.Domain;

namespace RiverBooks.Users.UsersEndpoints;

internal class Create(UserManager<ApplicationUser> userManager) 
    : Endpoint<CreateUserRequest>
{
    public override void Configure()
    {
        Post("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var newUser = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email
        };
        
        var result = await userManager.CreateAsync(newUser, request.Password);

        await SendOkAsync(cancellation: cancellationToken);
    }
}
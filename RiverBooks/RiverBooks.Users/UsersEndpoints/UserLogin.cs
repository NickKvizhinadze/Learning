using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;
using RiverBooks.Users.Entities;

namespace RiverBooks.Users.UsersEndpoints;

internal class UserLogin(UserManager<ApplicationUser> userManager) 
    : Endpoint<UserLoginRequest>
{
    public override void Configure()
    {
        Post("/users/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UserLoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            await SendUnauthorizedAsync(cancellation: cancellationToken);
            return;
        }

        var loginSuccessful = await userManager.CheckPasswordAsync(user, request.Password);
        
        if (!loginSuccessful)
        {
            await SendUnauthorizedAsync(cancellation: cancellationToken);
            return;
        }
        
        var jwtSecret = Config["Auth:JwtSecret"];

        var token = JwtBearer.CreateToken(p =>
        {
            p.SigningKey = jwtSecret!;
            p.User.Claims.Add(new Claim("EmailAddress", user.Email!));
        });

        await SendOkAsync(token, cancellation: cancellationToken);
    }
}
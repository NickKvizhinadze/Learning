using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace Moves.Api.Sdk.Consumer;

public class AuthTokenProvider
{
    private readonly HttpClient _httpClient;
    private string _cachedToken;
    private static readonly SemaphoreSlim Lock = new(1, 1);

    public AuthTokenProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrWhiteSpace(_cachedToken))
        {
            return _cachedToken;
        }

        await Lock.WaitAsync();
        try
        {
            if (!string.IsNullOrWhiteSpace(_cachedToken))
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(_cachedToken);
                var expireTimeStamp = token.Claims.Single(c => c.Type == "exp").Value;
                var expireDateTime = UnixTimeStampToDateTime(int.Parse(expireTimeStamp));
            }

            var response = await _httpClient.PostAsJsonAsync("https://localhost:5003/token", new
            {
                userid = "d8566de3-b1a6-4a9b-b842-8e3887a82e81",
                email = "nick@kvizhinadze.com",
                customClaims = new Dictionary<string, object>
                {
                    { "admin", true },
                    { "trusted_member", true }
                }
            });

            response.EnsureSuccessStatusCode();
            var newToken = await response.Content.ReadAsStringAsync();
            _cachedToken = newToken;
            return newToken;
        }
        finally
        {
            Lock.Release();
        }
    }

    private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
}
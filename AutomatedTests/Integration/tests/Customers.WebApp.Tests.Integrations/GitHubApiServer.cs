using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Customers.WebApp.Tests.Integrations;

public class GitHubApiServer : IDisposable
{
    private WireMockServer _server;
    public string Url => _server.Url!;

    public void Start()
    {
        _server = WireMockServer.Start(9850);
    }

    public void SetupUser(string username)
    {
        _server.Given(Request.Create()
                .WithPath($"/users/{username}")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithBody(GenerateResponseBody(username))
                .WithHeader("content-type", "application/json; charset=utf-8")
                .WithStatusCode(200));
    }


    public void SetupThrottledUser(string username)
    {
        _server.Given(Request.Create()
                .WithPath($"/users/{username}")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithBody(
                    @"{
    ""message"": ""API rate limit exceeded for 188.129.191.52. (But here's the good news: Authenticated requests get a higher rate limit. Check out the documentation for more details.)"",
    ""documentation_url"": ""https://docs.github.com/rest/overview/resources-in-the-rest-api#rate-limiting""
}")
                .WithHeader("content-type", "application/json; charset=utf-8")
                .WithStatusCode(403));
    }

    public void Dispose()
    {
        _server.Stop();
        _server.Dispose();
    }

    private static string GenerateResponseBody(string username)
    {
        return @$"{{
  ""login"": ""{username}"",
  ""id"": 12714018,
  ""node_id"": ""MDQ6VXNlcjEyNzE0MDE4"",
  ""avatar_url"": ""https://avatars.githubusercontent.com/u/12714018?v=4"",
  ""gravatar_id"": """",
  ""url"": ""https://api.github.com/users/{username}"",
  ""html_url"": ""https://github.com/{username}"",
  ""followers_url"": ""https://api.github.com/users/{username}/followers"",
  ""following_url"": ""https://api.github.com/users/{username}/following{{/other_user}}"",
  ""gists_url"": ""https://api.github.com/users/{username}/gists{{/gist_id}}"",
  ""starred_url"": ""https://api.github.com/users/{username}/starred{{/owner}}{{/repo}}"",
  ""subscriptions_url"": ""https://api.github.com/users/{username}/subscriptions"",
  ""organizations_url"": ""https://api.github.com/users/{username}/orgs"",
  ""repos_url"": ""https://api.github.com/users/{username}/repos"",
  ""events_url"": ""https://api.github.com/users/{username}/events{{/privacy}}"",
  ""received_events_url"": ""https://api.github.com/users/{username}/received_events"",
  ""type"": ""User"",
  ""site_admin"": false,
  ""name"": ""Nick Kvizhinadze"",
  ""company"": null,
  ""blog"": ""http://kvizhinadze.net"",
  ""location"": ""Georgia, Tbilisi"",
  ""email"": null,
  ""hireable"": null,
  ""bio"": ""Over the last 8 years, I have successfully completed several web-based development projects using ASP.NET Core, ASP.NET MVC, Angular, React, and other."",
  ""twitter_username"": null,
  ""public_repos"": 24,
  ""public_gists"": 1,
  ""followers"": 7,
  ""following"": 3,
  ""created_at"": ""2015-06-02T12:33:28Z"",
  ""updated_at"": ""2024-05-29T03:31:05Z""
}}";
    }
}
using GymManagement.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace GymManagement.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();

        return app;
    }
}
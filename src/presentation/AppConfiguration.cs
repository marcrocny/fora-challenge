using Fora.Challenge.Impl;

namespace Fora.Challenge.Api;

public static class AppConfiguration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddInfrastructure();
        return services;
    }

    public static void AddAppMiddleware(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
    }
}

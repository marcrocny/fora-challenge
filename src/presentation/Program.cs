using Fora.Challenge.Api;
using Fora.Challenge.Api.Endpoint;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAppServices();

        var app = builder.Build();

        app.AddAppMiddleware();
        app.AddWeatherEndpoint();
        app.AddFundabilityEndpoint();

        await app.RunAsync();
    }
}


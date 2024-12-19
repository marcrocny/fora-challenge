using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fora.Challenge.Impl.Edgar.Test;

[Trait("class", "integration")]
public class EdgarIntegrationShould
{
    [Fact]
    public async Task GetCompanyInfo()
    {
        var provider = (new ServiceCollection()).AddInfrastructure().BuildServiceProvider();
        var sut = provider.GetRequiredService<EdgarService>();

        // act
        var result = await sut.GetEdgarInfo("18926", default);

        // assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Startup()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddInfrastructure();
            }).Build();

        // act 
        CancellationTokenSource cts = new();
        Task hostTask = host.RunAsync(cts.Token);

        // assert

        var companyService = host.Services.GetRequiredService<ICompanyAnnualService>();

        var companies = await companyService.GetAll();
        Assert.NotNull(companies);

        cts.Cancel();
        await hostTask;
    }
}

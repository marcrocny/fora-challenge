using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace Fora.Challenge.Impl
{
    /// <summary>
    /// DependencyInjection (DI) extensions.
    /// </summary>
    public static class DI
    {
        /// <summary>
        /// For simplicity, everything for the project.
        /// </summary>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddHttpClient<Edgar.EdgarService>(httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://data.sec.gov/");

                httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "PostmanRuntime/7.34.0");   // ??? but okay
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "*/*");
            });

            services.AddHostedService<CompanyAnnualStartup>();
            
            services.AddSingleton<CompanyAnnualMemoryService>();
            services.AddSingleton<ICompanyMemoryStore>(p => p.GetRequiredService<CompanyAnnualMemoryService>());
            services.AddSingleton<ICompanyAnnualService>(p => p.GetRequiredService<CompanyAnnualMemoryService>());
            return services;
        }
    }
}

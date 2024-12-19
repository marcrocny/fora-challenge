using Fora.Challenge.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Fora.Challenge.Api.Endpoint;

public static class Fundability
{
    record FundabilityVM(int Id, string Name, decimal StandardFundableAmount, decimal SpecialFundableAmount);

    public static void AddFundabilityEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/fundability", async (ICompanyAnnualService companySvc, [FromQuery]string? startsWith) =>
            {
                var companies = string.IsNullOrEmpty(startsWith)
                    ? await companySvc.GetAll()
                    : await companySvc.GetFiltered(startsWith);

                var vms = companies.Select(Map);
                return vms;
            })
            .WithName("GetFundabilityAllFiltered");

        //routes.MapGet("/fundability")


    }

    static FundabilityVM Map(Company company)
    {
        var (standard, special) = FundabilityUtil.ComputeFundableAmounts(company);
        return new FundabilityVM(company.Id, company.Name, standard, special);
    }
}

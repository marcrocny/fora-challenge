using System.Text.RegularExpressions;
using Fora.Challenge.Entity;
using Microsoft.Extensions.Hosting;

namespace Fora.Challenge.Impl;

public class CompanyAnnualStartup : BackgroundService
{
    private const string FormOfInterest = "10-K";
    private readonly ICompanyMemoryStore companyMemoryStore;
    private readonly Edgar.EdgarService edgar;

    public CompanyAnnualStartup(ICompanyMemoryStore companyMemoryStore, Edgar.EdgarService edgar)
    {
        this.companyMemoryStore = companyMemoryStore;    
        this.edgar = edgar;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // NB: Blocks service until list is populated. This is only for a known, relatively small finite list.
        // For a longer list or periodic reload recycling or cache expiration strategy could be used.
        //var infos = await Task.WhenAll(Constants.EdgarCiks.Select(cik => edgar.GetEdgarInfo($"{cik}", stoppingToken)));

        SemaphoreSlim semaphore = new SemaphoreSlim(5,5);
        var companies = await Task.WhenAll(Constants.EdgarCiks.Select(cik => GetCompanyInfo(semaphore, $"{cik}", stoppingToken)));

        companyMemoryStore.Load(companies.Where(c => c != null).Cast<Company>());
    }

    private async Task<Company?> GetCompanyInfo(SemaphoreSlim semaphore, string cik, CancellationToken ct)
    {
        await semaphore.WaitAsync();
        try
        {
            var edgarInfo = await edgar.GetEdgarInfo(cik, ct);
            if (edgarInfo == null) return null;
            return GetCompany(edgarInfo);
        }
        finally
        {
            semaphore.Release();
        }
    }

    private static readonly Regex FrameMatch = new("^CY(\\d{4})$");

    public static Company GetCompany(Edgar.EdgarCompanyInfo edgarInfo) {

        var annuals = edgarInfo.Facts?.UsGaap?.NetIncomeLoss?.Units?.Usd?
            .Where(u => u.Form == FormOfInterest && u.Frame != null && FrameMatch.IsMatch(u.Frame))
            .Select(u => new AnnualIncome(Year: int.Parse(FrameMatch.Match(u.Frame).Groups[1].ValueSpan), Income: u.Val))
            .ToList();
        return new()
        {
            AnnualIncome = annuals ?? [],
            Cik = edgarInfo.Cik,
            Id = edgarInfo.Cik,
            Name = edgarInfo.EntityName,
        };
    }
}

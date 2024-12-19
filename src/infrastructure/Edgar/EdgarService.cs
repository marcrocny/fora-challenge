using System.Net.Http.Json;

namespace Fora.Challenge.Impl.Edgar;

/// <summary>
/// Get Edgar info
/// </summary>
public class EdgarService(HttpClient httpClient)
{
    private readonly HttpClient httpClient = httpClient;

    public async Task<EdgarCompanyInfo?> GetEdgarInfo(string companyId, CancellationToken ct)
    {
        var response = await httpClient.GetAsync(CompanyInfoPath(companyId), ct);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<EdgarCompanyInfo>(ct);
        }
        
        return null; // gulp
    }

    public static string CompanyInfoPath(string companyId) 
        => $"api/xbrl/companyfacts/CIK{companyId.PadLeft(10, '0')}.json";
}

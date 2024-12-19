using Fora.Challenge.Entity;

namespace Fora.Challenge.Impl;

/// <summary>
/// Implemented as an in-memory collection, updated at startup.
/// </summary>
/// <remarks>
/// Notes:
///  * Should implement pagination.
///  * 
/// </remarks>
public class CompanyAnnualMemoryService : ICompanyAnnualService, ICompanyMemoryStore
{
    private TaskCompletionSource isReady = new();
    private List<Company> companyStore = [];

    public async ValueTask<IEnumerable<Company>> GetAll()
    {
        await isReady.Task;
        return companyStore;
    }

    public async ValueTask<IEnumerable<Company>> GetFiltered(string startsWith)
    {
        await isReady.Task;
        return companyStore.Where(c => c.Name.StartsWith(startsWith));

        throw new NotImplementedException();
    }

    /// <summary>
    /// Bulk one-time load of available companies.
    /// </summary>
    /// <remarks>
    /// This could be refactored to use another strategy without affecting the primary Service contract.
    /// </remarks>
    void ICompanyMemoryStore.Load(IEnumerable<Company> companies)
    {
        companyStore = companies.ToList();
        isReady.SetResult();
    }
}

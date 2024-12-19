namespace Fora.Challenge;

public interface ICompanyAnnualService
{
    ValueTask<IEnumerable<Entity.Company>> GetAll();

    ValueTask<IEnumerable<Entity.Company>> GetFiltered(string startsWith);
}

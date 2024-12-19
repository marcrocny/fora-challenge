namespace Fora.Challenge.Entity;

public class Company
{
    public int Id { get; init; }

    public int Cik { get; init; }

    public string Name { get; init; } = "";

    public List<AnnualIncome> AnnualIncome { get; init; } = [];
}

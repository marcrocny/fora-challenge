using Fora.Challenge.Entity;

namespace Fora.Challenge;

public class FundabilityShould
{
    [Theory]
    [MemberData(nameof(FundabilityExpectations), MemberType = typeof(FundabilityShould))]
    public void ReturnExpectedFundabilityResult(Company company, (decimal, decimal) expected)
    {
        Assert.True(FundabilityUtil.ComputeFundableAmounts(company) == expected);
    }

    public static TheoryData<Company, (decimal, decimal)> FundabilityExpectations => new()
    {
        { new Company(), (0,0) },
        { new Company {AnnualIncome = []}, (0,0) },
        { new Company {AnnualIncome = [ new AnnualIncome(2018, 5m) ]}, (0,0) },
        { new Company {AnnualIncome = [ 
            new AnnualIncome(2018, 5m),
            new AnnualIncome(2019, 5m),
            new AnnualIncome(2020, 5m),
            new AnnualIncome(2021, 5m),
            new AnnualIncome(2022, 5m),
            ]}, 
            (5m * 0.2151m, 5m * 0.2151m) },
        { new Company {AnnualIncome = [
            new AnnualIncome(2018, 50_000_000_000m),
            new AnnualIncome(2019, 5m),
            new AnnualIncome(2020, 5m),
            new AnnualIncome(2021, 5m),
            new AnnualIncome(2022, 5m),
            ]},
            (50_000_000_000m * 0.1233m, 50_000_000_000m * 0.1233m) },
        { new Company {AnnualIncome = [
            new AnnualIncome(2018, 50_000_000_000m),
            new AnnualIncome(2019, 5m),
            new AnnualIncome(2020, 5m),
            new AnnualIncome(2021, 5m),
            new AnnualIncome(2022, -1m),
            ]},
            (0m, 0m) },
        { new Company {AnnualIncome = [
            new AnnualIncome(2018, 50_000_000_000m),
            new AnnualIncome(2019, -1m),
            new AnnualIncome(2020, 5m),
            new AnnualIncome(2021, 5m),
            new AnnualIncome(2022, 6m),
            ]},
            (50_000_000_000m * 0.1233m, 50_000_000_000m * 0.1233m) },
        { new Company {AnnualIncome = [
            new AnnualIncome(2018, 5m),
            new AnnualIncome(2019, 5m),
            new AnnualIncome(2020, 5m),
            new AnnualIncome(2021, 5m),
            new AnnualIncome(2022, 4m),
            ]},
            (5m * 0.2151m, 5m * 0.2151m * 0.75m) },
        { new Company {Name = "bob", AnnualIncome = [
            new AnnualIncome(2018, 5m),
            new AnnualIncome(2019, 5m),
            new AnnualIncome(2020, 5m),
            new AnnualIncome(2021, 5m),
            new AnnualIncome(2022, 5m),
            ]},
            (5m * 0.2151m, 5m * 0.2151m) },
        { new Company {Name = "alice", AnnualIncome = [
            new AnnualIncome(2018, 5m),
            new AnnualIncome(2019, 5m),
            new AnnualIncome(2020, 5m),
            new AnnualIncome(2021, 5m),
            new AnnualIncome(2022, 5m),
            ]},
            (5m * 0.2151m, 5m * 0.2151m * 1.15m) },
        { new Company {Name = "bob", AnnualIncome = [
            new AnnualIncome(2018, 5m),
            new AnnualIncome(2019, 5m),
            new AnnualIncome(2020, 5m),
            new AnnualIncome(2021, 5m),
            new AnnualIncome(2022, 4m),
            ]},
            (5m * 0.2151m, 5m * 0.2151m * 0.75m) },
        { new Company {Name = "alice", AnnualIncome = [
            new AnnualIncome(2018, 5m),
            new AnnualIncome(2019, 5m),
            new AnnualIncome(2020, 5m),
            new AnnualIncome(2021, 5m),
            new AnnualIncome(2022, 4m),
            ]},
            (5m * 0.2151m, 5m * 0.2151m * 0.75m * 1.15m) },
        // ...
    };

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        // ...
    };

}

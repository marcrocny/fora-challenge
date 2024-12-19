using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fora.Challenge.Entity;

namespace Fora.Challenge;

public static class FundabilityUtil
{
    /// <summary>
    /// Percentages expressed as multipliers. "If income is less than $10B,standard fundable amount is 21.51% of income.
    /// </summary>
    const decimal FundableRatio = 0.2151m;

    /// <summary>
    /// "If income is greater than or equal to $10B, standard fundable amount is 12.33% of income."
    /// </summary>
    const decimal FundableAboveRatio = 0.1233m;

    /// <summary>
    /// Cutoff ($10B).
    /// </summary>
    const decimal FundableCutoff = 10_000_000_000m;

    /// <summary>
    /// If the company name starts with a vowel, add 15% to the standard funding amount.
    /// </summary>
    const decimal SpecialVowelRatio = 1.15m;

    /// <summary>
    /// If the company’s 2022 income was less than their 2021 income, subtract 25%.
    /// </summary>
    const decimal SpecialYoyDecreaseRatio = 0.75m;

    /// <summary>
    /// Compute fundability amounts.
    /// </summary>
    /// <remarks>
    /// Note: This is hard-coded. Some questions:
    ///  * Future: Should the multipliers be configurable? How?
    ///  * Future: Should the 5y frame of interest "slide" with the passage of time? With what logic?
    ///  * Confirm: This logic causes a large discontinuity at cutoff. Should the cutoff computation be "21.51% of all income below $10B, and 12.33% of income above $10B"?
    ///  * Confirm: special ratios combine (see below).
    /// </remarks>
    public static (decimal standard, decimal special) ComputeFundableAmounts(Company company)
    {
        if (company == null) return (0, 0);
        var annuals = company.AnnualIncome;

        if (annuals == null || !annuals.Any()) return (0,0);

        var yearsOfInterest = annuals.Where(a => a.Year >= 2018 && a.Year <= 2022).ToList();
        if (yearsOfInterest.Count > 5) return (0,0);

        // inefficient, but only 5 members
        if (Enumerable.Range(2018, 5).Any(y => !yearsOfInterest.Any(a => a.Year == y))) return (0,0);
        var income2021 = yearsOfInterest.First(a => a.Year == 2021).Income;
        var income2022 = yearsOfInterest.First(a => a.Year == 2022).Income;

        if (income2021 <= 0 || income2022 <= 0) return (0,0);

        // now compute
        var maxIncome = yearsOfInterest.Select(a => a.Income).Max();
        var standard = maxIncome * (maxIncome >= FundableCutoff ? FundableAboveRatio : FundableRatio);

        // HACK: Vowel detection is English only.
        var vowels = "aeiouAEIOU";
        var special = vowels.Any(v => company.Name.StartsWith(v)) ? (standard * SpecialVowelRatio) : standard;

        // NOTE: would confirm, but assuming -25% combines with "vowel spiff"
        if (income2022 < income2021) special *= SpecialYoyDecreaseRatio;

        return (standard, special);
    }
}

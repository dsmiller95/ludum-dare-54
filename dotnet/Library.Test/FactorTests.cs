using DotnetLibrary.Audience.Factors;
using Godot;

namespace DotnetLibrary.Test;

public class FactorTests
{
    private static float ExpectedNormalizedVal(int totalSamples)
    {
        return 1f / Mathf.Sqrt(totalSamples);
    }
    
    [Fact]
    public void FactorShouldInitializeToZeros()
    {
        var factors = new Factors(0f);
        var expectedNormal = ExpectedNormalizedVal(Factors.TotalFactors);
        factors.GetNormalized(FactorType.Rage).Should().Be(expectedNormal);
        factors.GetNormalized(FactorType.Stupor).Should().Be(expectedNormal);
        factors.GetNormalized(FactorType.IntrovertToExtrovert).Should().Be(expectedNormal);
    }
    [Fact]
    public void FactorShouldInitializeToOnesAndNormalize()
    {
        var factors = new Factors(1f);
        var expectedNormal = ExpectedNormalizedVal(Factors.TotalFactors);
        factors.GetNormalized(FactorType.Rage).Should().Be(expectedNormal);
        factors.GetNormalized(FactorType.Stupor).Should().Be(expectedNormal);
        factors.GetNormalized(FactorType.IntrovertToExtrovert).Should().Be(expectedNormal);
    }
    [Fact]
    public void FactorShouldInitializeToTensAndNormalize()
    {
        var factors = new Factors(10f);
        var expectedNormal = ExpectedNormalizedVal(Factors.TotalFactors);
        factors.GetNormalized(FactorType.Rage).Should().Be(expectedNormal);
        factors.GetNormalized(FactorType.Stupor).Should().Be(expectedNormal);
        factors.GetNormalized(FactorType.IntrovertToExtrovert).Should().Be(expectedNormal);
    }
    [Fact]
    public void FactorShouldInitializeToTenthsAndNormalize()
    {
        var factors = new Factors(0.1f);
        var expectedNormal = ExpectedNormalizedVal(Factors.TotalFactors);
        factors.GetNormalized(FactorType.Rage).Should().Be(expectedNormal);
        factors.GetNormalized(FactorType.Stupor).Should().Be(expectedNormal);
        factors.GetNormalized(FactorType.IntrovertToExtrovert).Should().Be(expectedNormal);
    }
    [Fact]
    public void FactorShouldInitializeToOnesAndAddSomeRage()
    {
        var factors = new Factors(1f);
        var expectedNormal = ExpectedNormalizedVal(Factors.TotalFactors);
        factors.GetNormalized(FactorType.Rage).Should().Be(expectedNormal);
        factors.GetNormalized(FactorType.Stupor).Should().Be(expectedNormal);

        factors.AddFactor(FactorType.Rage, 1f);
        var newLength = Mathf.Sqrt(Factors.TotalFactors + 3);
        factors.GetNormalized(FactorType.Rage).Should().Be(2f / newLength);
        factors.GetNormalized(FactorType.Stupor).Should().Be(1f / newLength);
    }
}
namespace DotnetLibrary.Audience.Factors;

public interface IProvideFactorAccumulation
{
    public FactorAccumulation GetAccumulation();
}

public struct FactorAccumulation
{
    internal readonly float[] AccumulationsPerSecond;

    public FactorAccumulation(float defaultAccumulation)
    {
        this.AccumulationsPerSecond = new float[5];
        Array.Fill(AccumulationsPerSecond, defaultAccumulation);
    }
    public void AccumulateFactor(FactorType factor, float amount, TimeSpan? per = null)
    {
        per ??= TimeSpan.FromSeconds(1);
        AccumulationsPerSecond[(int)factor] = amount / (float)per.Value.TotalSeconds;
    }
    
}
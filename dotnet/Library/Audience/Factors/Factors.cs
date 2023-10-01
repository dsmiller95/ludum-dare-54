namespace DotnetLibrary.Audience.Factors;

public struct Factors
{
    private float[] factors;
    private float[] normalizedFactors;
    private bool needsNormalize;

    public void Normalize()
    {
        throw new NotImplementedException();
    }

    public float GetNormalized(FactorType factor)
    {
        if(needsNormalize) this.Normalize();
        return normalizedFactors[(int)factor];
    }

    public void AddFactor(FactorType factor, float amount)
    {
        factors[(int)factor] += amount;
        needsNormalize = true;
    }

    public void DecayFactors(float deltaTime)
    {
        throw new NotImplementedException();
        needsNormalize = true;
    }

    public Factors(float defaultFactor)
    {
        this.factors = new float[5];
        this.normalizedFactors = new float[5];
        Array.Fill(factors, defaultFactor);
        needsNormalize = true;
    }
}

public enum FactorType
{
    Rage = 0,
    MotorControl = 1,
    Stinky = 2,
    Horny = 3,
    Wallflower = 4
}
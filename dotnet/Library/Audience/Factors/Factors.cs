using Godot;

namespace DotnetLibrary.Audience.Factors;

public struct Factors
{
    private float[] factors;
    private float[] normalizedFactors;
    private bool needsNormalize;

    private void Normalize()
    {
        float sum = 0;
        
        for (int i = 0; i < factors.Length; i++)
        { // this can be tuned. doesn't need to be cartesian normalization, could be linear/average
            sum += factors[i] * factors[i];
            normalizedFactors[i] = factors[i];
        }

        for (int i = 0; i < factors.Length; i++)
        {
            normalizedFactors[i] /= sum;
        }

        needsNormalize = false;
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

    public void DecayFactors(float deltaTime, float decayConstant)
    {
        var exponentialDecay = Mathf.Pow(Mathf.E, -decayConstant * deltaTime);
        for (int i = 0; i < factors.Length; i++)
        {
            factors[i] *= exponentialDecay;
        }
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
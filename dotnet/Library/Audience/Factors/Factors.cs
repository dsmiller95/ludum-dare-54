using System.Diagnostics;
using Godot;

namespace DotnetLibrary.Audience.Factors;

public struct Factors
{
    public static readonly int TotalFactors = 5;
    
    private float[] factors;
    private float[] normalizedFactors;
    private bool needsNormalize;

    public float[] GetRawFactorsUnnormalized()
    {
        return factors;
    }
    
    private void Normalize()
    {
        float sum = 0;
        
        for (int i = 0; i < factors.Length; i++)
        { // this can be tuned. doesn't need to be cartesian normalization, could be linear/average
            sum += factors[i] * factors[i];
            normalizedFactors[i] = factors[i];
        }

        float distance = Mathf.Sqrt(sum);

        for (int i = 0; i < factors.Length; i++)
        {
            normalizedFactors[i] /= distance;
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

    public void AccumulateFactors(FactorAccumulation accumulation, float deltaTime)
    {
        for (int i = 0; i < factors.Length; i++)
        {
            factors[i] += accumulation.AccumulationsPerSecond[i] * deltaTime;
        }
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
        if (defaultFactor == 0)
        {
            defaultFactor = 0.0001f;
        }
        this.factors = new float[5];
        this.normalizedFactors = new float[5];
        Array.Fill(factors, defaultFactor);
        needsNormalize = true;
    }
}

/// <summary>
/// Because factors are normalized, the positive version should be the most interesting/ effect-based.
/// I.E. "Rage" is the factor, because a value of 1 produces strong punches. a Rage value of 0 does almost nothing.
/// 
/// </summary>
/// <remarks>
/// Some factors may be bimodal, that's fine. but if one side is the exclusive "Action taken" value, use that name. 
/// </remarks>
public enum FactorType
{
    /// <summary>
    /// 0..1
    /// 1 means the actor is ready to punch
    /// </summary>
    Rage = 0,
    /// <summary>
    /// 0..1
    /// 1 means the actor ambles around aimlessly
    /// </summary>
    Stupor = 1,
    /// <summary>
    /// -1..1
    /// </summary>
    StinkyToAttractive = 2,
    /// <summary>
    /// 0..1
    /// how affected an actor is by the stinky matrix. 0 means they are not affected at all.
    /// 1 means they are strongly attracted to attractive, and repelled from stinky.
    /// </summary>
    Horny = 3,
    /// <summary>
    /// -1..1
    /// </summary>
    IntrovertToExtrovert = 4
}
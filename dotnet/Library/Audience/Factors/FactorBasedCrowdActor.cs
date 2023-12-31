using Godot;

namespace DotnetLibrary.Audience.Factors;

public class FactorBasedCrowdActor
{
    private readonly IFactorEffect[] effects;
    private readonly FactorTuningParams tuning;
    private readonly FactorAccumulation accumulation;
    private readonly IProvideFactorOverride? overrideSource;

    public Factors factors;
    
    public float[] GetRawFactorsUnnormalized()
    {
        return factors.GetRawFactorsUnnormalized();
    }

    private AiResult lastAiResult;
    
    public FactorBasedCrowdActor(
        IFactorEffect[] effects,
        FactorTuningParams tuning,
        FactorAccumulation accumulation,
        IProvideFactorOverride? overrideSource = null)
    {
        this.effects = effects;
        this.tuning = tuning;
        this.accumulation = accumulation;
        factors = overrideSource?.GetOverrideFactor() ?? new Factors(0.5f);
        this.overrideSource = overrideSource is { UseLiveOverride: true } ? overrideSource : null;
    }

    public void UpdateFactors(float deltaTime)
    {
        if (overrideSource is { UseLiveOverride: true })
        {
            this.factors = overrideSource.GetOverrideFactor();
        }
        else
        {
            this.factors.AccumulateFactors(accumulation, deltaTime);
            this.factors.DecayFactors(deltaTime, tuning.FactorDecayRate);
        }

        this.factors.HintNormalizeNow();
    }
    
    public void Update(float deltaTime, float currentSeconds, Span<AiNeighbor?> neighbors)
    {   
        var aiParams = new AiParams
        {
            deltaTime = deltaTime,
            currentTime = currentSeconds,
            SelfFactors = this.factors,
            Neighbors = neighbors,
        };
        Span<AiResult> allEffects = stackalloc AiResult[effects.Length];
        for (int i = 0; i < effects.Length; i++)
        {
            allEffects[i] = effects[i].GetFactorEffect(aiParams);
        }

        var sumEffects = allEffects[0];
        for (int i = 1; i < allEffects.Length; i++)
        {
            sumEffects += allEffects[i];
        }

        lastAiResult = sumEffects;
    }

    public void ReceivePushEvent(PushEvent pushEvent)
    {
        var rageEffect = pushEvent.PushForce.Length() / tuning.PushToRageRatio;
        factors.AddFactor(FactorType.Rage, rageEffect);
    }

    public float GetFirmness()
    {
        return lastAiResult.Firmness;
    }

    public Vector2 GetCurrentSelfMoveForce()
    {
        return lastAiResult.AdditionalLinearForce;
    }

    public CrowdActorEffect GetCrowdEffectLevels()
    {
        return new CrowdActorEffect
        {
            EnragedEffect = factors.GetNormalized(FactorType.Rage),
            DrunkEffect = factors.GetNormalized(FactorType.Stupor)
        };
    }
}
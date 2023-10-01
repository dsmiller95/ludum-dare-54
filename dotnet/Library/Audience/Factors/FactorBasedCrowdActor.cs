using Godot;

namespace DotnetLibrary.Audience.Factors;

public class FactorBasedCrowdActor : ICrowdActor
{
    private readonly IFactorEffect[] effects;
    private readonly FactorTuningParams tuning;
    private readonly FactorAccumulation accumulation;
    private readonly IProvideFactorOverride? overrideSource;

    private Factors factors;
    
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
        this.overrideSource = overrideSource;
        factors = overrideSource?.GetOverrideFactor() ?? new Factors(0.5f);
    }
    
    public void Update(double deltaTime, double currentSeconds, NeighborCrowdActor[] neighbors)
    {
        if (overrideSource is { UseLiveOverride: true })
        {
            this.factors = overrideSource.GetOverrideFactor();
        }
        else
        {
            this.factors.AccumulateFactors(accumulation, (float)deltaTime);
            this.factors.DecayFactors((float)deltaTime, tuning.FactorDecayRate);
        }
        var aiParams = new AiParams
        {
            deltaTime = (float)deltaTime,
            currentTime = (float)currentSeconds,
            SelfFactors = this.factors,
            Neighbors = neighbors? // TODO: cache array
                .Select(x =>
                {
                    if (x.actor is not FactorBasedCrowdActor facBased) return (AiNeighbor?)null;
                    return new AiNeighbor
                    {
                        Position = x.relativePosition,
                        Factors = facBased.factors
                    };
                }).ToArray() ?? Array.Empty<AiNeighbor?>(),
        };
        var allEffects = new AiResult[effects.Length];
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
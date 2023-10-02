using DotnetLibrary.Audience;
using DotnetLibrary.Audience.Factors;
using Godot;

namespace LudumDare54.Audience;

[GlobalClass]
public partial class FactorBasedCrowdActorConfig : Resource, ICrowdActorPreset
{
    [Export] public FactorBasedCrowdActorTuning Tuning;
    [Export] public FactorOverrideSource OverrideSource;
    [Export] public FactorArchetype Archetype;
    
    public FactorBasedCrowdActor ConstructConfiguredActor(RandomNumberGenerator rng)
    {
        var tuningParams = Tuning.FactorTuningParams();
        var accumulationParams = Archetype.GetAccumulation();
        var effects = new IFactorEffect []
        {
            new MotorControlFactor(tuningParams, rng),
            new RageFactor(tuningParams, rng),
            new AttractorControlFactor(tuningParams.HornyTuning!, new HornyControlFactorAttractorStrategy()),
            new AttractorControlFactor(tuningParams.ExtrovertTuning!, new ExtrovertControlFactorAttractorStrategy()),
        };
        return new FactorBasedCrowdActor(effects, tuningParams, accumulationParams, OverrideSource);
    }
}
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
    
    public ICrowdActor ConstructConfiguredActor()
    {
        var tuningParams = Tuning.FactorTuningParams();
        var accumulationParams = Archetype.GetAccumulation();
        var rng = new RandomNumberGenerator();
        var effects = new IFactorEffect []
        {
            new MotorControlFactor(tuningParams, rng),
            new RageFactor(tuningParams, rng)
        };
        return new FactorBasedCrowdActor(effects, tuningParams, accumulationParams, OverrideSource);
    }
}
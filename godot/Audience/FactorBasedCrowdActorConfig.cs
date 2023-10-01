using DotnetLibrary.Audience;
using DotnetLibrary.Audience.Factors;
using Godot;

namespace LudumDare54.Audience;

[GlobalClass]
public partial class FactorBasedCrowdActorConfig : Resource, ICrowdActorPreset
{
    [Export] public float RandomWalkJitter = 2f;
    [Export] public float PushToRageRatio = 400f;
    public ICrowdActor ConstructConfiguredActor()
    {
        var tuningParams = new FactorTuningParams
        {
            RandomWalkJitter = RandomWalkJitter,
            PushToRageRatio = PushToRageRatio
        };
        var rng = new RandomNumberGenerator();
        var effects = new IFactorEffect []
        {
            new MotorControlFactor(tuningParams, rng)
        };
        return new FactorBasedCrowdActor(effects, tuningParams);
    }
}
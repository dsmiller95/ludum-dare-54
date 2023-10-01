using DotnetLibrary.Audience.Factors;
using Godot;

namespace LudumDare54.Audience;

[GlobalClass]
public partial class FactorBasedCrowdActorTuning : Resource
{
    [Export] public float FactorDecayRate = 0.1f;
    [Export] public float RandomWalkJitter = 2f;
    
    [ExportGroup("Rage")]
    [Export] public float PushToRageRatio = 400f;
    [Export] public float RagePunchChancePerSecond = 1f;
    [Export] public float RagePunchMinMagnitude = 1f;
    [Export] public float RagePunchMaxMagnitude = 100f;
    [Export] public float RagePunchDuration = 0.1f;

    public FactorTuningParams FactorTuningParams()
    {
        var tuningParams = new FactorTuningParams
        {
            RandomWalkJitter = RandomWalkJitter,
            FactorDecayRate = FactorDecayRate,
            PushToRageRatio = PushToRageRatio,
            RagePunchChancePerSecond = RagePunchChancePerSecond,
            RagePunchMinMagnitude = RagePunchMinMagnitude,
            RagePunchMaxMagnitude = RagePunchMaxMagnitude,
            RagePunchDuration = RagePunchDuration
        };
        return tuningParams;
    }
}
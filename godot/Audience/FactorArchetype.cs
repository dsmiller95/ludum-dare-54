using DotnetLibrary.Audience.Factors;
using Godot;

namespace LudumDare54.Audience;


[GlobalClass]
public partial class FactorArchetype : Resource, IProvideFactorAccumulation
{
    [Export] public float RandomMultiplier = 0.1f;
    [Export] public float GlobalMultiplier = 1f;
    [Export] public float RagePerSecond = 0f;
    [Export] public float StuporPerSecond = 0f;
    [Export] public float StinkyToAttractivePerSecond = 0f;
    [Export] public float HornyPerSecond = 0f;
    [Export] public float IntrovertToExtrovertPerSecond = 0f;
    
    public FactorAccumulation GetAccumulation()
    {
        var randomAccumulation = new FactorAccumulation(0f);
        randomAccumulation.AccumulateFactor(FactorType.Rage, GD.Randf());
        randomAccumulation.AccumulateFactor(FactorType.Stupor, GD.Randf());
        randomAccumulation.AccumulateFactor(FactorType.StinkyToAttractive, GD.Randf());
        randomAccumulation.AccumulateFactor(FactorType.Horny, GD.Randf());
        randomAccumulation.AccumulateFactor(FactorType.IntrovertToExtrovert, GD.Randf());
        randomAccumulation.NormalizeAllAccumulates(RandomMultiplier);
        
        var accumulation = new FactorAccumulation(0f);
        accumulation.AccumulateFactor(FactorType.Rage, RagePerSecond);
        accumulation.AccumulateFactor(FactorType.Stupor, StuporPerSecond);
        accumulation.AccumulateFactor(FactorType.StinkyToAttractive, StinkyToAttractivePerSecond);
        accumulation.AccumulateFactor(FactorType.Horny, HornyPerSecond);
        accumulation.AccumulateFactor(FactorType.IntrovertToExtrovert, IntrovertToExtrovertPerSecond);
        accumulation.NormalizeAllAccumulates(GlobalMultiplier);
        
        
        accumulation.AddAll(randomAccumulation);
        
        return accumulation;
    }
}
using DotnetLibrary.Audience.Factors;
using Godot;

namespace LudumDare54.Audience;


[GlobalClass]
public partial class FactorArchetype : Resource, IProvideFactorAccumulation
{
    [Export] public float RagePerSecond = 0f;
    [Export] public float StuporPerSecond = 0f;
    [Export] public float StinkyToAttractivePerSecond = 0f;
    [Export] public float HornyPerSecond = 0f;
    [Export] public float IntrovertToExtrovertPerSecond = 0f;
    
    public FactorAccumulation GetAccumulation()
    {
        var accumulation = new FactorAccumulation(0f);
        accumulation.AccumulateFactor(FactorType.Rage, RagePerSecond);
        accumulation.AccumulateFactor(FactorType.Stupor, StuporPerSecond);
        accumulation.AccumulateFactor(FactorType.StinkyToAttractive, StinkyToAttractivePerSecond);
        accumulation.AccumulateFactor(FactorType.Horny, HornyPerSecond);
        accumulation.AccumulateFactor(FactorType.IntrovertToExtrovert, IntrovertToExtrovertPerSecond);
        return accumulation;
    }
}
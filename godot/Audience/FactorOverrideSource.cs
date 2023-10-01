using DotnetLibrary.Audience.Factors;
using Godot;

namespace LudumDare54.Audience;


[GlobalClass]
public partial class FactorOverrideSource : Resource, IProvideFactorOverride
{
    [Export] public float Rage = 0f;
    [Export] public float Stupor = 0f;
    [Export] public float StinkyToAttractive = 0f;
    [Export] public float Horny = 0f;
    [Export] public float IntrovertToExtrovert = 0f;
    
    public Factors GetOverrideFactor()
    {
        var factors = new Factors(0f);
        factors.AddFactor(FactorType.Rage, Rage);
        factors.AddFactor(FactorType.Stupor, Stupor);
        factors.AddFactor(FactorType.StinkyToAttractive, StinkyToAttractive);
        factors.AddFactor(FactorType.Horny, Horny);
        factors.AddFactor(FactorType.IntrovertToExtrovert, IntrovertToExtrovert);
        return factors;
    }
}
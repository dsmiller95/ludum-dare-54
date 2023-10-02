using DotnetLibrary.Audience;
using DotnetLibrary.Audience.Factors;
using Godot;

namespace LudumDare54.Audience;

public interface ICrowdActorPreset
{
    public FactorBasedCrowdActor ConstructConfiguredActor();
}
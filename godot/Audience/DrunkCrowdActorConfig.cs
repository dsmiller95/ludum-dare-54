using DotnetLibrary.Audience;
using Godot;

namespace LudumDare54.Audience;

[GlobalClass]
public partial class DrunkCrowdActorConfig : Resource, ICrowdActorPreset
{
    [Export] public float randomnessSpeed = 2f;
    public ICrowdActor ConstructConfiguredActor()
    {
        return new RandomCrowdActor(randomnessSpeed);
    }
}
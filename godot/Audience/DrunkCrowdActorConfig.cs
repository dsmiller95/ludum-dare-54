using DotnetLibrary.Audience;
using Godot;

namespace LudumDare54.Audience;

[GlobalClass]
public partial class DrunkCrowdActorConfig : Resource, ICrowdActorPreset
{
    [Export] public float friction = 10f;
    [Export] public float randomnessSpeed = 2f;
    public ICrowdActor ConstructConfiguredActor()
    {
        return new RandomCrowdActor(friction, randomnessSpeed);
    }
}
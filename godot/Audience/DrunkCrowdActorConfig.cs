using DotnetLibrary.Audience;
using Godot;

namespace LudumDare54.Audience;

[GlobalClass]
public partial class DrunkCrowdActorConfig : Resource
{
    [Export] public float randomnessSpeed = 2f;
    public RandomCrowdActor ConstructConfiguredActor()
    {
        return new RandomCrowdActor(randomnessSpeed);
    }
}
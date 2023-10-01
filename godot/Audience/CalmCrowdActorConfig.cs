using DotnetLibrary.Audience;
using Godot;

namespace LudumDare54.Audience;

[GlobalClass]
public partial class CalmCrowdActorConfig : Resource, ICrowdActorPreset
{
    [Export] private float calmLevel = 0.5f;
    [Export] private float frictionMultiplier = 0.1f;
    public ICrowdActor ConstructConfiguredActor()
    {
        return new CalmCrowdActor(calmLevel, frictionMultiplier);
    }
}